<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();
    $user;
    $success;

    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser_users']) {
            header("Location: ../myhome.php");
            die();
        }
        else if (isset($_SESSION['admin'])) {
            $admin=true;
        }
    }
    else {
        header("Location: ../login.php");
        die();
    }

    if (isset($_GET['user'])) {
        $user = db_inspect_user($_GET['user']);
    }
    else if (isset($_POST['id'])) {
        $user = $_POST;//Kopioidaan POST-muuttujan tiedot muuttujaan

        if ($_SESSION['admin']) {
            if (db_edit_user_admin($user)) {
                $success = true;
            }
            else {
                $success = false;
            }
        }
        else {
            if (db_edit_user($user)) {
                $success = true;
            }
            else {
                $success = false;
            }
        }


        $user = db_inspect_user($_POST['id']);
    }

    if (!isset($user)) {
        header("Location: ../index.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Editing <?php echo $user['username'];?></title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Edit <?php echo $user['username'];?></h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            <input style="display: none;" type='text' name="id" value="<?php echo $user['id'];?>" readonly>
            Username(Read only):<br><input type="text" name="username" maxlength="40" value="<?php echo $user['username'];?>" readonly><br>
            First Name:<br><input type="text" name="firstname" maxlength="50" value="<?php echo $user['first_name'];?>"><br>
            Last Name:<br><input type="text" name="lastname" maxlength="50" value="<?php echo $user['last_name'];?>"><br>
            Address:<br><input type="text" name="address" maxlength="50" value="<?php echo $user['user_address'];?>"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" value="<?php echo $user['user_postalcode'];?>"><br>
            City:<br><input type="text" name="city" maxlength="40" value="<?php echo $user['user_city'];?>"><br>
            Email:<br><input type="text" name="email" maxlength="80" value="<?php echo $user['user_email'];?>" required><br>
            Phone:<br><input type="text" name="phone" maxlength="20" value="<?php echo $user['user_phone'];?>"><br><br>
            <?php
                if ($_SESSION['admin']) {
                    $superuser = "";
                    $superuser_products = "";
                    $superuser_suppliers = "";
                    $superuser_orders = "";
                    $superuser_users = "";

                    if ($user['superuser']) {
                        $superuser='checked';
                    }

                    if ($user['superuser_products']) {
                        $superuser_products='checked';
                    }

                    if ($user['superuser_suppliers']) {
                        $superuser_suppliers='checked';
                    }

                    if ($user['superuser_orders']) {
                        $superuser_orders='checked';
                    }
                    
                    if ($user['superuser_users']) {
                        $superuser_users='checked';
                    }

                    echo 'Administrative permissions:<br>
                    <input type="checkbox" name="superuser" value="yes" ' . $superuser . '>Read order, product, supplier and user info<br>
                    <input type="checkbox" name="superuser_products" value="yes" ' . $superuser_products . '>Add/Edit products<br>
                    <input type="checkbox" name="superuser_suppliers" value="yes" ' . $superuser_suppliers . '>Add/Edit suppliers<br>
                    <input type="checkbox" name="superuser_orders" value="yes" ' . $superuser_orders . '>Add/Edit orders<br>
                    <input type="checkbox" name="superuser_users" value="yes" ' . $superuser_users . '>Add/Edit users<br>';
                }
            ?>
            <input type="submit" value="Save changes">
        </form>
        <?php
            //Ilmoitetaan käyttäjälle onnistuiko tietojen päivitys
            if (isset($success)) {
                echo '<br>';
                if ($success) {
                    echo 'User updated successfully!';
                }
                else {
                    echo 'Failed to update user!';
                }
            }
        ?>
        <p><a href="users.php">RETURN TO USER LIST</a></p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>