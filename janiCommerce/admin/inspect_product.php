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

    $product;
    
    if (isset($_GET['product'])) {
        $product = db_inspect_product($_GET['product']);
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>
        <?php echo $store['name']?> - 
            <?php
                if(isset($product)) {
                    echo 'Details of ' . $product['product_name'];
                }
                else {
                    echo "Product not found";
                }
            ?>
        </title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>
            <?php
                //Jos tuote löytyy niin ilmoitetaan mitä tuotetta katsellaan ja jos ei niin kerrotaan että tuotetta ei löytynyt
                if(isset($product)) {
                    echo "Details of " . $product['product_name'] . ":";
                }
                else {
                    echo "The product was not found";
                }
            ?>
        </h1>
            <?php
                //Jos tuote löytyi niin näytetään sen speksit
                if(isset($product)) {
                    if ($product['visible'] == false) {
                        echo '<p><strong>THIS PRODUCT IS CURRENTLY HIDDEN!</strong></p>';
                    }
                    echo "<p>Name: " . $product['product_name'] . "<br>" .
                    "Selling Price: " . $product['sellprice'] . "€<br>" .
                    "Buying Price: " . $product['buyprice'] . "€<br>" .
                    "Stock: " . $product['stock'] . "<br>" .
                    "Product ID: " . $product['product_id'] . "<br>" .
                    "EAN Code: " . $product['ean_code'] . "<br>" .
                    "Supplier: ";

                    if (isset($product['supplier'])) {
                        echo db_get_supplier_name($product['supplier']);
                    }
                    else {
                        echo 'NOT SET';
                    }

                    echo "<br>Suppliers Product ID: " . $product['supplier_product_id'] . "<br><br>
                    <br>Description:<br> " . $product['product_desc'] . '</p>';
                    if (isset($product['supplier'])) {
                        $otherproducts_url = 'supplier_products.php?supplier=' . $product['supplier'];
                        echo '<a href="' . $otherproducts_url . '">OTHER PRODUCTS BY THIS SUPPLIER</a><br>';
                    }
                    if ($_SESSION['superuser_products']) {
                        $edit_url = 'edit_product.php?product=' . $product['product_id'];
                        echo '<a href="' . $edit_url . '">EDIT THIS PRODUCT</a><br>';
                    }

                }
                echo '<a href="suppliers.php">TO SUPPLIERS PAGE</a><br><a href="products.php">TO PRODUCTS PAGE</a></p>';
            ?>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>