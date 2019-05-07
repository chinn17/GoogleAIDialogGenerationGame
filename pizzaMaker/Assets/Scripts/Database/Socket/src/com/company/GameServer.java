    package src.com.company;

    import java.util.*;
    import java.util.concurrent.TimeUnit;
    import src.com.company.MySQLHandler;
    import static src.com.company.Variables.*;

    class Variables {
        static List<String> Users_LoggedIn = new ArrayList<>();
        static List<String> Users_StartGame = new ArrayList<>();
        // Host : Guest
        public static Map<String, String> CurrentGames = new HashMap<>();
        static Map<String, String> PreGame = new HashMap<>();
    }

    public class GameServer {

        public ArrayList<Player> players;
        public ArrayList<Game> games;

        //tracks the last time a player has requested anything in epoch
        //private ArrayList<Long> player_last_request = new ArrayList<Long>();
        private HashMap<String, Long> player_last_request = new HashMap<String, Long>();


        //keeps track of game_ids
        private int game_counter = 0;

        String param_delimiter = "|";

        MySQLHandler sql_handler;
        Thread gameThread;

        Queue<String> responseList1 = new LinkedList<String>() ;
        Queue<String> responseList2 = new LinkedList<String>() ;

        public GameServer()
        {
            players = new ArrayList<Player>();
            games = new ArrayList<Game>();

            sql_handler = new MySQLHandler();
            System.out.println("Initialized SQL handler");


            manageGames();

        }

        //receives request in form of uri, and returns response in form of string
        public String processRequest(String resourceString)
        {
            String endpoint;
            String queryString = null;
            String endpointParts[] = resourceString.split("\\?");


            //separates endpoint from parameters
            endpoint = endpointParts[0];
            if (endpointParts.length == 2) {
                queryString = endpointParts[1];
            }

            //System.out.println("Endpoint: "+endpoint);
            //System.out.println("queryString: "+queryString);


            //response to return to client
            String to_return = "";

            //separates parameters
            String[] parameters = new String[0];
            String[] values = new String[0];
            try {
                try
                {
                    parameters = queryString.split("&");
                    values = new String[parameters.length];
                    for (int i = 0; i < parameters.length; i++) {
                        String[] argSplit = parameters[i].split("=");
                        values[i] = argSplit[1];
                    }
                } catch(Exception ex){}

                //System.out.println(endpoint.toLowerCase());



                //processes the request for different endpoints
                switch(endpoint.toLowerCase()){
                    case "/login":
                        //[0] = username
                        //[1] = password
                        to_return = "101"+sql_handler.login(values[0], values[1]);
                        break;
                    case "/register":
                        //[0] = username
                        //[1] = email
                        //[2] = password
                        to_return = "102"+sql_handler.register(values[0],values[1],values[2]);
                        break;
                    case "/chat":
                        to_return = "103"+sendMessage(values[0], values[1]);
                        break;
                    case "/addtocart":
                        to_return = "104"+addToCart(values[0]);
                        break;
                    case "/gametimer":
                        //[0] = session_id
                        //[1] = game_id
                        to_return = "105"+"Time Remaining";
                        break;
                    case "/pay":
                        to_return = "106"+sendScore();
                        break;
                    case "/discount":
                        to_return = "107"+applyDiscount(values[0],values[1]);
                        break;



                    case "/startgame":
                        String userWhoStartedGame = values[0];
                        Users_StartGame.add(userWhoStartedGame);
                        System.out.println(Users_StartGame);
                        while (true) {
                            if (PreGame.containsKey(userWhoStartedGame)) {
                                CurrentGames.put(userWhoStartedGame, PreGame.get(userWhoStartedGame));
                                PreGame.remove(userWhoStartedGame);
                                to_return = "108" + String.format("User connected");
                                break;
                            }
                        }

                        break;

                    case "/joingame":
                        String userWhoIsJoiningGame = values[0];
                      String usernameOfGameWeAreTryingToJoin = values[1];
                      if (Users_StartGame.contains(usernameOfGameWeAreTryingToJoin)) {
                          System.out.println("Request to Join Game");
                          PreGame.put(usernameOfGameWeAreTryingToJoin, userWhoIsJoiningGame);
                          Users_StartGame.remove(usernameOfGameWeAreTryingToJoin);
                          to_return = "109PlayerConnected";
                      } else {
                          to_return = "109Error";
                      }
                        break;

                    case "/heartbeat":
                        to_return = "110"+getLatestResponse(values[0]);
                        break;

                    default:
                        break;

                }

            } catch(Exception ex){
                System.out.println("Error: "+ex.toString());
            }

            //returns newline if error so that client doesn't freeze
            if(to_return.equals(""))
                to_return="\n";

            return to_return;
        }

    public String getLatestResponse(String playerId) {

        if (playerId.equals("player1") && !responseList1.isEmpty()) {
            return responseList1.poll();
        } else if (playerId.equals("player2") && !responseList2.isEmpty()) {
            return responseList2.poll();
        }

    return "No Updates";

    }

        public String sendMessage(String newMessage, String playerId) {
            if (playerId.equals("player1")) {
                System.out.println("Player 1 sending message");
                responseList2.add("103"+newMessage);
            } else if (playerId.equals("player2")) {
                System.out.println("Player 2 sending message");
                responseList1.add("103"+newMessage);
            }


            return "Message Received, Response List Updated";
        }

        public String addToCart (String itemName) {

            responseList2.add("104"+itemName);

            return "Added To Cart";
        }

        public String sendScore () {
            responseList1.add("106");
            return "Sending Score to Maker";
        }

        public String applyDiscount (String discount, String cartItemNumber) {

            return "Applying Discount";
        }


        public void manageGames()
        {
            //creates new thread to manage games so that it doesn't interfere with requests
            gameThread = new Thread() {

                public void run() {

                    //loadGames();

                    int seconds = 0;
                    while (true) {

                        //System.out.println("Manage Games second: " + seconds);


                        //prints out currently running games
                        if(games.size()>0) {
                            System.out.println("-- Current games --");
                            int counter = 0;
                            while(counter < games.size())
                            {
                                Game game = games.get(counter);
                                System.out.println("game "+game.getGameID()+": ["+game.map_name+"] ["+game.getNumPlayers()+"] ["+game.getWaitLeft()+"|"+game.getSecondsLeft()+"]");

                                //removes game if no one is playing
                                if(game.getNumPlayers()==0) {
                                    games.remove(counter);
                                    continue;
                                }
                                else
                                    counter++;

                                //if game finished, then restart it with new map
                                if(game.hasFinished())
                                    game.restartGame();
                            }
                        }


    //                    if(games.size()>0) {
    //                        System.out.println("## Current Games ##");
    //
    //                        //prints the current games
    //                        for (int x = 0; x < games.size(); x++)
    //                            System.out.println(games.get(x).toString());
    //                    }




                        //handles players and disconnects ones that haven't sent request for a while
                        //checks every minute
                        if(seconds%60==0) {
                            long cur_milliseconds = System.currentTimeMillis();
                            int counter = 0;
                            while (counter < players.size()) {
                                String session_id = players.get(counter).getSessionID();

                                double seconds_passed = (cur_milliseconds - player_last_request.get(session_id)) / 1000;

                                //disconnect after 1 day
                                if (seconds_passed >= 86400)
                                    removePlayer(players.get(counter).getSessionID());
                                else
                                    counter++;
                            }
                        }

                        seconds++;

                        //System.out.println();
                        try {
                            TimeUnit.SECONDS.sleep(1);
                        } catch (Exception ex) {
                            System.out.println("Timeout ERROR: " + ex.toString());
                        }
                    }
                }

            };

            gameThread.start();
        }


        //returns the amount of time left to wait and amount of time left in game
        public String getGameTimer(String session_id, int game_id)
        {
            //update last request time for current player
            updatePlayerRequestTime(session_id);

            Game game = getGameById(game_id);

            String to_return = "";
            to_return += game.getWaitLeft() + param_delimiter;
            to_return += game.getSecondsLeft();

            return to_return;
        }

        //joins a game of map_name
        public String joinGame(String session_id, String map_name)
        {
            System.out.println("joinGame()");

            //update last request time for current player
            updatePlayerRequestTime(session_id);


            //gets player associated with provided session_id
            Player player = getPlayer(session_id);

            //if no player exists with session_id, don't join game
            if(player==null)
                return "";

            System.out.println(session_id+" is joining "+map_name);

            int game_index = -1;
            boolean found_game = false;
            for(int x = 0; x < games.size(); x++)
            {
                Game game = games.get(x);

                //if map name matches, and game hasn't started, and the game isn't full, then join
                if(game.map_name.equals(map_name) && game.hasGameStarted()==false && game.getNumPlayers()<game.getMaxPlayers())
                {
                    game_index = x;
                    found_game = true;
                    break;
                }
            }


            Game game;

            //if found a game to join, then join it
            if(found_game)
            {
                System.out.println("Game found");
                game = games.get(game_index);
                game.addPlayer(player);
            }
            //if no found game, then create it
            else
            {
                System.out.println("Game not found, so create one");
                game = new Game(game_counter);
                game_counter++;


                game.map_name = map_name;
                game.addPlayer(player);
                game.startWait();

                games.add(game);
            }

            String to_return = Integer.toString(game.getGameID());

            to_return += param_delimiter;

            System.out.println("to_return: "+to_return);
            System.out.println("session_id: "+player.getSessionID());

            //returns new spawn coordinates
            to_return += game.getSpawnCoordinates(player.getSessionID());

            System.out.println("joinGame() returning: "+to_return);

            return to_return;
        }

        public String pickedUp(String session_id, int game_id, int pickupable_id)
        {
            //update last request time for current player
            updatePlayerRequestTime(session_id);

            Game game = getGameById(game_id);

            game.addPickupable(session_id, pickupable_id);

            ArrayList<Integer> new_pickupables = game.getNewPickupablesForPlayer(session_id);

            System.out.println("New pickupables: "+new_pickupables.toString());

            return "";
        }


        //returns the game ids of all currently running games
        public String getGames()
        {
            System.out.println("getGames()");

            String to_return = "";

            ArrayList<Integer> game_ids = new ArrayList<Integer>();
            for(int x = 0; x < games.size(); x++)
                game_ids.add(games.get(x).getGameID());

            Integer[] int_array = game_ids.toArray(new Integer[game_ids.size()]);

            return Arrays.toString(int_array);
        }


        public String sendMessage(String givenMessage){
            return "test";
        }


        //returns true if successful, which means if the player doesn't already exist
        public boolean addPlayer(Player player)
        {
            //checks that player doesn't already exist
            if(getPlayerIndex(player.getSessionID()) == -1)
            {
                //adds newly logged in player to list of active players
                players.add(player);
                //sets player's last request time
                player_last_request.put(player.getSessionID(), System.currentTimeMillis());

                return true;
            }
            else
                return false;


        }

        //removes player from the game server and any game they are part of
        public boolean removePlayer(String session_id)
        {
            int index = getPlayerIndex(session_id);


            //checks that player doesn't already exist
            if(index != -1)
            {
                players.remove(index);
                //removes player's last request time
                player_last_request.remove(index);

                //removes player from any games
                for(int x = 0; x < games.size(); x++)
                {
                    Game game = games.get(x);
                    game.removePlayer(session_id);
                }

                return true;
            }

            return false;
        }

        //returns a game based on its int id
        public Game getGameById(int game_id)
        {
            for(int x = 0; x < games.size(); x++)
            {
                if(games.get(x).getGameID() == game_id)
                    return games.get(x);
            }

            return null;
        }

        //returns player object that corresponds to the session_id
        public Player getPlayer(String session_id)
        {
            int index = getPlayerIndex(session_id);
            if(index!=-1)
                return players.get(index);
            else
                return null;
        }

        //update last request time for current player
        public void updatePlayerRequestTime(String session_id)
        {
            player_last_request.put(session_id, System.currentTimeMillis());
            //player_last_request.set(getPlayerIndex(session_id), System.currentTimeMillis());
        }

        //returns index of player in players global variable
        public int getPlayerIndex(String session_id)
        {
            for(int x = 0; x < players.size(); x++)
            {
                String cur_session_id = players.get(x).getSessionID();
                if(cur_session_id.equals(session_id)) {
                    return x;
                }
            }

            return -1;
        }
    }
