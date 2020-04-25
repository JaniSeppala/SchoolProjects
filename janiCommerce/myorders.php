<?php
    include 'database/database_connections.php';
    include 'config.php';
    session_start();
    if (!isset($_SESSION['id']) || !isset($_SESSION['password'])) {
        header("Location: login.php");
        die();
    }
    else {
        $orders = db_list_users_orders($_SESSION['id']);
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - My Orders</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>YOUR ORDERS</h1>
        <?php
            if (isset($orders)) {
                echo '<table border="1">
                <tr><td>INSPECT</td>
                </tr>';
                foreach ($orders as $row) {
                    $inspecturl = 'inspect_order.php?orderid=' . $row['id'];
                    echo '<tr><td><a href="' . $inspecturl . '">' . $row['order_datetime'] . '</a></td>
                    </tr>';
                }
                echo '</table>';
            }
            else {
                echo '<p>You have not placed any orders.</p>';
            }
        ?>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>