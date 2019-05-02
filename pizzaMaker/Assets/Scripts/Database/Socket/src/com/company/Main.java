package com.company;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;

public class Main {

    public static void main(String[] args) throws IOException {

        ServerSocket serverSocket = new ServerSocket(4343, 10);
        MySQLHandler sql_handler = new MySQLHandler();
        System.out.println("Server started");
        String toSend = "Empty";
        while (true) {
            Socket socket = serverSocket.accept();
            InputStream is = socket.getInputStream();
            OutputStream os = socket.getOutputStream();

            // Receiving
            byte[] lenBytes = new byte[4];
            is.read(lenBytes, 0, 4);
            int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) |
                    ((lenBytes[1] & 0xff) << 8) | (lenBytes[0] & 0xff));
            byte[] receivedBytes = new byte[len];
            is.read(receivedBytes, 0, len);
            String received = new String(receivedBytes, 0, len);


            System.out.println("Server received: " + received);
            String action = received.split("/")[0];
            String[] parameters = received.split("/")[1].split("%");
            switch (action) {
                case "login":
                    break;
                case "register":
                    toSend = sql_handler.register(parameters[1].split("=")[1], parameters[2].split("=")[1], parameters[3].split("=")[1]);

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

            socket.close();
            if (received.equals("Close")) {
                serverSocket.close();
                break;

            }
        }

    }

}