<?php
    session_start();
    include 'config.php';
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Order Placed Successfully</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>YOUR ORDER WAS PLACED SUCCESSFULLY!</h1>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>