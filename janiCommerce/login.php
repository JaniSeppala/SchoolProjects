<?php
    session_start();
    include 'config.php';
    $message = "";
    if (isset($_POST['username']) && isset($_POST['password'])) {
        include 'database/database_connections.php';
        $user=db_validate_user($_POST['username'], $_POST['password']);
        //Jos käyttäjä löytyy ja salasana on oikein niin tallennetaan käyttäjän tiedot sessio-muuttujiin
        if (isset($user)) {
            $_SESSION['id']=$user['id'];
            $_SESSION['username']=$user['username'];
            $_SESSION['password']=$user['userpassword'];
            $_SESSION['superuser']=$user['superuser'];
            $_SESSION['superuser_products']=$user['superuser_products'];
            $_SESSION['superuser_suppliers']=$user['superuser_suppliers'];
            $_SESSION['superuser_orders']=$user['superuser_orders'];
            $_SESSION['superuser_users']=$user['superuser_users'];
            $_SESSION['admin']=$user['administrator'];
            $_SESSION['firstname']=$user['first_name'];
            $_SESSION['lastname']=$user['last_name'];
            $_SESSION['address']=$user['user_address'];
            $_SESSION['postalcode']=$user['user_postalcode'];
            $_SESSION['city']=$user['user_city'];
            $_SESSION['email']=$user['user_email'];
            $_SESSION['phone']=$user['user_phone'];
            header("Location: myhome.php");
            die();
        }
        else {
            $message = '<p>INVALID USERNAME AND/OR PASSWORD!</p>';
        }
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Login</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>LOGIN TO YOUR ACCOUNT</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            USERNAME:<input type="text" name="username" required><br>
            PASSWORD:<input type="password" name="password" required><br>
            <input type="submit" value="Login"><br>
        </form>
        <?php echo $message;?>
        <?php
        include 'footer.php';
    ?>
    </body>
</html>