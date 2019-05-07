package src.com.company;


import java.sql.*;

public class MySQLHandler {

    Connection connection = null;

    public MySQLHandler() {
        connection = connect();
    }

    public Connection connect() {
        String host = "13.59.229.139";
        int port = 3306;
        String dbname = "googleaidb";
        String username = "googleai";
        String password = "!csc631!";


//        try {
//            Class.forName("com.mysql.cj.jdbc.Driver");
//        } catch (ClassNotFoundException e) {
//            e.printStackTrace();
//        }

//        Connection connection = null;
        try {
            connection = DriverManager.getConnection("jdbc:mysql://" + host + ":" + port + "/" + dbname, username, password);
            System.out.println("Connected");

        } catch (SQLException e) {
            e.printStackTrace();
        }

        return connection;
    }

    public int createGame() {
        int game_id = -1;


        try {
            String sql = "INSERT INTO games (players, num_players, map) VALUES (?,?,?)";
            PreparedStatement prepared = connection.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
            prepared.setString(1, "");
            prepared.setInt(2, 0);
            prepared.setString(3, "default");
            int result = prepared.executeUpdate();

            //if success
            if (result != 0) {
                //get the new game's id
                ResultSet rs = prepared.getGeneratedKeys();
                if (rs.next()) {
                    game_id = rs.getInt(1);
                }
            } else
                System.out.println("Error creating new game");

        } catch (Exception ex) {
            System.out.println("createGame() error: " + ex.toString());
        }

        return game_id;

    }

    //returns player object
    public String login(String username, String password) {

        //hashes provided password to compare to hashed passwords in database
        String hashed_password = Security.getHashedPassword(username, password);

        try {
            String sql = "SELECT * FROM users WHERE users.username=? AND users.password=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, username);
            prepared.setString(2, hashed_password);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next()) {
                int user_id = rs.getInt("user_id");
                //creates new player object for newly logged in player
                return "Success";
            } else
                System.out.println("No results");

        } catch (Exception ex) {
            return ex.getMessage();
        }
        return "Error";
    }

    //logs user out by removing their session ID, and removing them from active games
    public boolean logout(String session_id) {
        try {
            //remove session_id from users table
            String statement = "UPDATE users SET users.session_id=? WHERE users.session_id=?";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, "");
            prepared.setString(2, session_id);
            int result = prepared.executeUpdate();

            //if successful removing sesssion_id
            if (result != 0) {

                //removes session_id from any active games

                return true;
            }

        } catch (Exception ex) {

        }

        return false;
    }

    //returns boolean whether registration was successful
    public String register(String username, String password, String email) {
        //hashes provided password to compare to hashed passwords in database
        String hashed_password = Security.getHashedPassword(username, password);

        try {
            //insert session id into the database
            String statement = "INSERT INTO users (username, email, password) VALUES (?,?,?)";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, username);
            prepared.setString(2, email);
            prepared.setString(3, hashed_password);
            int result = prepared.executeUpdate();

            //if successfully insert row
            if (result != 0) {
                return "Success";
            }
            return "Error";
        } catch (Exception ex) {
            System.out.println("Register EX: " + ex);
            return (ex.getMessage());
        }

    }


    //returns boolean whether username is already taken
    public boolean doesUsernameExist(String username) {
        try {
            String sql = "SELECT user_id FROM users WHERE users.username=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, username);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next())
                return true;
            else
                return false;

        } catch (Exception ex) {

        }

        return false;
    }

    //returns boolean whether email is already taken
    public boolean doesEmailExist(String email) {
        try {
            String sql = "SELECT user_id FROM users WHERE users.email=?";
            PreparedStatement prepared = connection.prepareStatement(sql);
            prepared.setString(1, email);

            ResultSet rs = prepared.executeQuery();

            //if query returned results
            if (rs.next())
                return true;
            else
                return false;

        } catch (Exception ex) {

        }

        return false;
    }

    //updates
    private boolean insertSessionID(int user_id, String session_id) {
        try {
            //insert session id into the database
            String statement = "UPDATE users SET users.session_id=? WHERE users.user_id=?";
            PreparedStatement prepared = connection.prepareStatement(statement);
            prepared.setString(1, session_id);
            prepared.setInt(2, user_id);
            prepared.executeUpdate();

            return true;
        } catch (Exception ex) {
            System.out.println("updateSessionID() failed: " + session_id + " | " + ex.toString());
        }

        return false;
    }

}
