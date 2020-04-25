<?php
    include 'database/database_connections.php';
    include 'config.php';
    session_start();
    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (strlen($_SESSION['firstname']) > 0) {
            $name = $_SESSION['firstname'];
        }
        else {
            $name = $_SESSION['username'];
        }
    }
    else {
        header("Location: login.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - My Home</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Welcome <?php echo $name . '!';?></h1>
        <p>
            <a href="myorders.php">YOUR ORDERS</a><br>
            <a href="mycart.php">YOUR SHOPPING CART</a><br>
            <a href="logout.php">LOG OUT</a>
        </p>
        <?php
            if ($_SESSION['superuser']) {
                echo '<a href="admin/index.php">GO TO ADMIN PANEL</a>';
            }
        ?>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>