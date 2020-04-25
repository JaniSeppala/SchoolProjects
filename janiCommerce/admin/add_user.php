<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();

    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser_users']) {
            header("Location: ../myhome.php");
            die();
        }
    }
    else {
        header("Location: ../login.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Add User</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>ADD A USER</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            Username:<br><input type="text" name="username" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            Password:<br><input type="text" name="password" maxlength="40" placeholder="MAX 40 CHARACTERS" required><br>
            First Name:<br><input type="text" name="firstname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Last Name:<br><input type="text" name="lastname" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Address:<br><input type="text" name="address" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" placeholder="MAX 10 CHARACTERS"><br>
            City:<br><input type="text" name="city" maxlength="40" placeholder="MAX 40 CHARACTERS"><br>
            Email:<br><input type="text" name="email" maxlength="80" placeholder="MAX 80 CHARACTERS" required><br>
            Phone:<br><input type="text" name="phone" maxlength="20" placeholder="MAX 20 CHARACTERS"><br><br>
            <?php
                if ($_SESSION['admin']) {
                    echo '
                    Administrative permissions:<br>
                    <input type="checkbox" name="superuser" value="yes">Read order, product, supplier and user info<br>
                    <input type="checkbox" name="superuser_products" value="yes">Add/Edit products<br>
                    <input type="checkbox" name="superuser_suppliers" value="yes">Add/Edit suppliers<br>
                    <input type="checkbox" name="superuser_orders" value="yes">Delete orders<br>
                    <input type="checkbox" name="superuser_users" value="yes">Add/Edit users<br>';
                }
            ?>
            <input type="submit" value="Add User">

        </form><br>
        <?php
            //Jos lomake on täytetty ja lisäys-painiketta painettu niin lisätään tiedot tietokantaan
            if (isset($_POST['username']) && isset($_POST['password']) && isset($_POST['email'])) {
                if (db_user_exists($_POST['username'])) {
                    echo 'USERNAME ALREADY EXISTS!';
                }
                else {
                    $user = $_POST;//Kopioidaan POST-muuttujan tiedot uuteen muuttujaan

                    if ($_SESSION['admin']) {
                        if (db_add_user_admin($user)) {
                            echo 'USER ADDED SUCCESSFULLY!';
                        }
                        else {
                            echo 'Failed to add the user!';
                        }
                    }
                    else {
                        if (db_add_user($user)) {
                            echo 'USER ADDED SUCCESSFULLY!';
                        }
                        else {
                            echo 'Failed to add the user!';
                        }
                    }
                }
            }
        ?>
        <p><a href="users.php">RETURN TO USERLIST</a></p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>