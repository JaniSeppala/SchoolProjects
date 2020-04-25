<?php
    include 'config.php';
    session_start();
    $_SESSION = array();
    $message;
    if (session_destroy()) {
        $message = 'LOGGED OUT SUCCESSFULLY!';
    }
    else {
        $message = 'FAILED TO TERMINATE THE SESSION! CLOSE THE BROWSER TO FINNISH LOGGING OUT!';
    }

?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Logged Out</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <p>
            <?php echo $message;?>
        </p>
        <p>
            <a href="index.php">BACK TO HOMEPAGE</a><br>
            <a href="login.php">LOG IN AGAIN</a>
        </p>
        <?php
        include 'footer.php';
    ?>
    </body>
</html>