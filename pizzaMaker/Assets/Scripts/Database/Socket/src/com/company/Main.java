package com.company;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import static com.company.Variables.*;


class Variables {
    static List<String> Users_LoggedIn = new ArrayList<>();
    static List<String> Users_StartGame = new ArrayList<>();
    // Host : Guest
    public static Map<String, String> CurrentGames = new HashMap<>();
    static Map<String, String> PreGame = new HashMap<>();

}

public class Main {
    public static void main(String[] args) throws IOException {

        ServerSocket serverSocket = new ServerSocket(4343, 10);
        System.out.println("Main started");


        while (true) {
            Socket socket = null;
            try {
                socket = serverSocket.accept();
                System.out.print("Connected Client");
                InputStream is = socket.getInputStream();
                OutputStream os = socket.getOutputStream();
                Thread thread = new ClientWorker(socket, is, os);
                thread.start();
            } catch (Exception e) {
                if (socket != null) {
                    socket.close();
                }
                e.printStackTrace();
            }

        }

    }


    private static class ClientWorker extends Thread {

        final InputStream is;
        final OutputStream os;
        final Socket socket;

        public ClientWorker(Socket socket, InputStream is, OutputStream os) {
            this.socket = socket;
            this.os = os;
            this.is = is;
        }

        @Override
        public void run() {

            MySQLHandler sql_handler = new MySQLHandler();

            String[] emptyList = {"", ""};


            String toSend = "Empty";

            while (true) {

                try {


                    // Receiving
                    byte[] lenBytes = new byte[4];
                    is.read(lenBytes, 0, 4);
                    int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) |
                            ((lenBytes[1] & 0xff) << 8) | (lenBytes[0] & 0xff));
                    byte[] receivedBytes = new byte[len];
                    is.read(receivedBytes, 0, len);
                    String received = new String(receivedBytes, 0, len);


                    if (received.length() > 0) {
                        System.out.println("Main received: " + received);

                    }
                    if (received.equals("close")) {
                        this.socket.close();
                        break;

                    }
                    String action = received.split("/")[0];
                    String[] parameters = received.length() > 0 ? received.split("/")[1].split("%") : emptyList;

                    switch (action) {
                        case "login":
                            String username = parameters[1].split("=")[1];
                            String password = parameters[2].split("=")[1];
                            toSend = sql_handler.login(username, password);
                            if (toSend.equals("Success")) {
                                Users_LoggedIn.add(username);
                                System.out.println(Users_LoggedIn);
                            }
                            break;
                        case "register":
                            toSend = sql_handler.register(parameters[1].split("=")[1], parameters[2].split("=")[1], parameters[3].split("=")[1]);
                            break;
                        case "startgame":
                            String userWhoStartedGame = parameters[1].split("=")[1];
                            Users_StartGame.add(userWhoStartedGame);
                            System.out.println(Users_StartGame);
                            while (true) {
                                if (PreGame.containsKey(userWhoStartedGame)) {
                                    CurrentGames.put(userWhoStartedGame, PreGame.get(userWhoStartedGame));
                                    PreGame.remove(userWhoStartedGame);
                                    toSend = String.format("%s connected.", PreGame.get(userWhoStartedGame));
                                    break;
                                }
                            }
                            break;
                        case "joingame":
                            String userWhoIsJoiningGame = parameters[1].split("=")[1];
                            String usernameOfGameWeAreTryingToJoin = parameters[2].split("=")[1];
                            if (Users_StartGame.contains(usernameOfGameWeAreTryingToJoin)) {
                                PreGame.put(usernameOfGameWeAreTryingToJoin, userWhoIsJoiningGame);
                                Users_StartGame.remove(usernameOfGameWeAreTryingToJoin);
                                toSend = "PlayerConnected";
                            } else {
                                toSend = "Error";
                            }
                            break;
                    }

                    // Sending
                    byte[] toSendBytes = toSend.getBytes();
                    int toSendLen = toSendBytes.length;
                    byte[] toSendLenBytes = new byte[4];
                    toSendLenBytes[0] = (byte) (toSendLen & 0xff);
                    toSendLenBytes[1] = (byte) ((toSendLen >> 8) & 0xff);
                    toSendLenBytes[2] = (byte) ((toSendLen >> 16) & 0xff);
                    toSendLenBytes[3] = (byte) ((toSendLen >> 24) & 0xff);
                    os.write(toSendLenBytes);
                    os.write(toSendBytes);


                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

        }
    }
}

