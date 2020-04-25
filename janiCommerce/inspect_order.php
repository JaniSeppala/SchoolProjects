<?php
    include 'database/database_connections.php';
    include 'config.php';
    session_start();
    if (!isset($_SESSION['id']) || !isset($_SESSION['password'])) {
        header("Location: login.php");
        die();
    }
    else if (isset($_GET['orderid'])){
        $orderinfo = db_inspect_order($_GET['orderid'], $_SESSION['id']);
        $orderrows = db_get_order_rows($_GET['orderid']);
    }
    if (!isset($orderinfo) || !isset($orderrows)){
        header("Location: myhome.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Inspect Order</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>SHIPPING INFO</h1>
        <p>
        <?php
            echo 'First name: ' . $orderinfo['delivery_firstname'] . '<br>
            Last name: ' . $orderinfo['delivery_lastname'] . '<br>
            Address: ' . $orderinfo['delivery_address'] . '<br>
            Postal Code: ' . $orderinfo['delivery_postalcode'] . '<br>
            City: ' . $orderinfo['delivery_city'] . '<br>
            Email: ' . $orderinfo['delivery_email'] . '<br>
            Phone: ' . $orderinfo['delivery_phone'];
        ?>
        </p>
        <h1>PRODUCTS</h1>
        <?php
            echo '<table border="1">
            <tr>
            <td><strong>PRODUCT NAME</strong></td>
            <td><strong>PRICE</strong></td>
            <td><strong>QUANTITY</strong></td>
            <td><strong>TOTAL PRICE</strong></td>
            </tr>';
            $total = 0;

            foreach ($orderrows as $row) {
                echo "<tr>
                <td>" . $row['product_name'] . '</td>
                <td>' . $row['sellprice'] . '€</td>
                <td>' . $row['quantity'] . '</td>
                <td>' . $row['sellprice'] * $row['quantity'] . '€</td>
                </tr>';
                $total = $total + ($row['sellprice'] * $row['quantity']);
            }

            echo '</table>
            <p>TOTAL PRICE: '. $total . '€</p>';
        ?>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>