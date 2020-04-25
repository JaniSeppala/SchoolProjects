<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();
    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser']) {
            header("Location: ../myhome.php");
            die();
        }
    }
    else {
        header("Location: ../login.php");
        die();
    }

    $user;
    
    if (isset($_GET['user'])) {
        $user = db_inspect_user($_GET['user']);
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>
        <?php echo $store['name']?> - 
            <?php
                if(isset($user)) {
                    echo 'Details of ' . $user['username'];
                }
                else {
                    echo "User not found";
                }
            ?>
        </title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>
            <?php
                //Jos käyttäjä löytyy niin ilmoitetaan mitä käyttäjää katsellaan ja jos ei niin kerrotaan että käyttäjää ei löytynyt
                if(isset($user)) {
                    echo "Details of " . $user['username'] . ":";
                }
                else {
                    echo "The user was not found";
                }
            ?>
        </h1>
        <p>
            <?php
                //Jos käyttäjä löytyi niin näytetään sen speksit
                if(isset($user)) {
                    echo "Username: " . $user['username'] . "<br>" .
                    "First Name: " . $user['first_name'] . "<br>" .
                    "Last Name: " . $user['last_name'] . "<br>" .
                    "Address: " . $user['user_address'] . "<br>" .
                    "Postal Code:<br> " . $user['user_postalcode'] . "<br>" .
                    "City: " . $user['user_city'] . "<br>" .
                    "Email: " . $user['user_email'] . "<br>" .
                    "Phone: " . $user['user_phone'] . "<br><br>";
                    if ($_SESSION['superuser_users']) {
                        echo '<a href="edit_user.php?user=' . $user['id'] . '">EDIT THIS USER</a><br>';
                    }
                }

                echo '<a href="users.php">RETURN TO USER LIST</a>';
            ?>
        </p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>