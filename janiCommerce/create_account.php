<?php
    session_start();
    include 'config.php';
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Account Registration</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Account Registration</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            Username:<br><input type="text" name="username" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            Password:<br><input type="password" name="password" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            First Name:<br><input type="text" name="firstname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Last Name:<br><input type="text" name="lastname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Address:<br><input type="text" name="address" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" placeholder="MAX 10 CHARACTERS"><br>
            City:<br><input type="text" name="city" maxlength="40" placeholder="MAX 40 CHARACTERS"><br>
            Email:<br><input type="text" name="email" maxlength="80" placeholder="MAX 80 CHARACTERS" required><br>
            Phone:<br><input type="text" name="phone" maxlength="20" placeholder="MAX 20 CHARACTERS"><br><br>
            <input type="submit" value="Create Account">
        </form><br>
        <?php
            include 'database/database_connections.php';
            //Jos lomake on täytetty ja lisäys-painiketta painettu niin lisätään tiedot tietokantaan
            if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['email'])) {
                if (db_user_exists($_POST['username'])) {
                    echo 'USERNAME ALREADY EXISTS!';
                }
                else {
                    $user = $_POST;//Kopioidaan POST-muuttujan tiedot uuteen muuttujaan
                    if (db_add_user($user)) {
                        echo 'ACCOUNT CREATED SUCCESSFULLY!<br>
                        <a href="login.php">GO TO LOGIN PAGE</a>';
                    }
                    else {
                        echo 'Failed to add the user!';
                    }
                }
            }
        ?>
        <p><a href="index.php">RETURN TO HOMEPAGE</a></p>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>