<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();
    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser']) {
            header("Location: ../myhome.php");
            die();
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
        <title><?php echo $store['name']?> - Admin Panel</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Admin Panel</h1>
        <p>
            Actions:<br>
            <a href="products.php">INSPECT PRODUCTS</a><br>
            <a href="suppliers.php">INSPECT SUPPLIERS</a><br>
            <a href="orders.php">INSPECT ORDERS</a><br>
            <a href="users.php">INSPECT USERS</a><br>
            <?php
                if ($_SESSION['superuser_products']) {
                    echo '<a href="add_product.php">ADD A PRODUCT</a><br>';
                }
                if ($_SESSION['superuser_suppliers']) {
                    echo '<a href="add_supplier.php">ADD A SUPPLIER</a><br>';
                }
                if ($_SESSION['superuser_users']) {
                    echo '<a href="add_user.php">ADD A USER</a><br>';
                }
            ?>
        </p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>