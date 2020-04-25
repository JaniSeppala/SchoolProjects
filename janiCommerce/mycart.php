<?php
    include 'database/database_connections.php';
    include 'config.php';
    session_start();

?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Shopping Cart</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Your Shopping Cart</h1>
        <?php
            if (isset($_POST['product']) && isset($_POST['quantity'])) {
                if (!isset($_SESSION['cart'])) {
                    $_SESSION['cart'] = array();
                }
                $product = db_inspect_product($_POST['product']);
                $cart_item = array(
                    'id' => $product['id'],
                    'product_id' => $product['product_id'],
                    'product_name' => $product['product_name'],
                    'quantity' => $_POST['quantity'],
                    'sellprice' => $product['sellprice']
                );
                array_push($_SESSION['cart'], $cart_item);
            }

            if (isset($_POST['edit']) && isset($_POST['quantity'])) {
                $_SESSION['cart'][$_POST['edit']]['quantity'] = $_POST['quantity'];
            }

            if (isset($_POST['remove'])) {
                    unset($_SESSION['cart'][$_POST['remove']]);
            }

            if (isset($_SESSION['cart']) && sizeof($_SESSION['cart']) > 0) {
                echo '<table border="1">
                <tr>
                <td><strong>PRODUCT NAME</strong></td>
                <td><strong>PRICE</strong></td>
                <td><strong>QUANTITY</strong></td>
                <td><strong>TOTAL PRICE</strong></td>
                </tr>';
                $index = 0;
                $total = 0;

                foreach ($_SESSION['cart'] as $row) {
                    echo "<tr>
                    <td>" . $row['product_name'] . '</td>
                    <td>' . $row['sellprice'] . '€</td>
                    <td><form action="' . $_SERVER['PHP_SELF'] . '" method="post">
                    <input style="display: none;" type="text" name="edit" value="' . $index . '" readonly>
                    <input type="number" name="quantity" min="1" value="' . $row['quantity'] . '" step="1">
                    <input type="submit" value="Edit">
                    </form></td>
                    <td>' . $row['sellprice'] * $row['quantity'] . '€</td>
                    <td><form action="' . $_SERVER['PHP_SELF'] . '" method="post">
                    <input style="display: none;" type="text" name="remove" value="' . $index . '" readonly>
                    <input type="submit" value="Remove">
                    </form></td>
                    </tr>';
                    $index = $index + 1;
                    $total = $total + ($row['sellprice'] * $row['quantity']);
                }

                echo '</table>
                <p>TOTAL PRICE: '. $total . '€</p>
                <a href="checkout.php">TO CHECKOUT</a>';
            }
            else {
                echo 'You have no products in your shopping cart.';
            }

            echo '<br><a href="index.php">CONTINUE SHOPPING</a>';
        ?>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>