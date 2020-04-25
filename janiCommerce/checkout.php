<?php
    include 'database/database_connections.php';
    include 'config.php';
    session_start();

    if (!isset($_SESSION['cart']) || sizeof($_SESSION['cart']) < 1) {
        header("Location: mycart.php");
        die();
    }

    if (isset($_POST['firstname'])) {
        $order = $_POST;
        if (isset($_SESSION['id'])) {
            $order['user_id']=$_SESSION['id'];
        }
        $rows = $_SESSION['cart'];
        db_add_order($order, $rows);
        unset($_SESSION['cart']);
        header("Location: order_placed.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Checkout</title>
    </head>
    <body>
        <?php
            include 'navigation.php';
        ?>
        <a href="index.php">CONTINUE SHOPPING</a><br>
        <h1>Order Info</h1>
        <?php
            echo '<table border="1">
            <tr>
            <td><strong>PRODUCT NAME</strong></td>
            <td><strong>PRICE</strong></td>
            <td><strong>QUANTITY</strong></td>
            <td><strong>TOTAL PRICE</strong></td>
            </tr>';
            $total = 0;

            foreach ($_SESSION['cart'] as $row) {
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
        <h1>Shipping Info</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            First Name:<br><input type="text" name="firstname" maxlength="50" value="<?php if(isset($_SESSION['firstname'])){echo $_SESSION['firstname'];}?>" required><br>
            Last Name:<br><input type="text" name="lastname" maxlength="50" value="<?php if(isset($_SESSION['lastname'])){echo $_SESSION['lastname'];}?>" required><br>
            Address:<br><input type="text" name="address" maxlength="50" value="<?php if(isset($_SESSION['address'])){echo $_SESSION['address'];}?>" required><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" value="<?php if(isset($_SESSION['postalcode'])){echo $_SESSION['postalcode'];}?>" required><br>
            City:<br><input type="text" name="city" maxlength="40" value="<?php if(isset($_SESSION['city'])){echo $_SESSION['city'];}?>" required><br>
            Email:<br><input type="email" name="email" maxlength="80" value="<?php if(isset($_SESSION['email'])){echo $_SESSION['email'];}?>" required><br>
            Phone:<br><input type="text" name="phone" maxlength="20" value="<?php if(isset($_SESSION['phone'])){echo $_SESSION['phone'];}?>"><br><br>
            <input type="submit" value="Place Order">
        </form>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>