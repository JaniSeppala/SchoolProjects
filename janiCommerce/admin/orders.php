<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();
    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser']) {
            header("Location: ../myhome.php");
            die();
        }
        else if (isset($_POST['remove'])) {
            db_delete_order($_POST['remove']);
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
        <title><?php echo $store['name']?> - Orders</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Orders</h1>
        <?php
            $orders = db_list_orders();
            //Jos tilauksia löytyy niin tulostetaan ne taulukkoon
            if (isset($orders)) {
                echo '<table border="1">
                <tr>
                <td><strong>DATE</strong></td>
                <td><strong>INSPECT</strong></td>';
                if ($_SESSION['superuser_orders']) {
                    echo '<td><strong>REMOVE ORDER</strong></td>';
                }
                echo '</tr>';
                foreach ($orders as $row) {
                    $inspect_url = 'inspect_order.php?order=' . $row['id'];
                    echo "<tr>
                    <td>" . $row['order_datetime'] . '</td>
                    <td><a href="' . $inspect_url .'">INSPECT ORDER</a></td>';
                    if ($_SESSION['superuser_orders']) {
                        echo '<td>
                        <form action="' . $_SERVER['PHP_SELF'] . '" method="post">
                        <input style="display: none;" type="number" name="remove" value="' . $row['id'] . '" readonly>
                        <input type="submit" value="Delete Order">
                        </form>
                        </td>';
                    }
                    echo '</tr>';
                }
                echo '</table>';
            }
            //Jos tilauksia ei löydy niin ilmoitetaan siitä
            else {
                echo "No orders found";
            }
            
        ?>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>