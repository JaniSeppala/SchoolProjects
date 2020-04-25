<?php
    session_start();
    include 'config.php';
    $product;
    if (isset($_GET['product'])) {
        include 'database/database_connections.php';
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
                if(isset($product) && $product['visible'] == true) {
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
                if(isset($product) && $product['visible'] == true) {
                    echo "Details of " . $product['product_name'] . ":";
                }
                else {
                    echo "The product was not found";
                }
            ?>
        </h1>
        <p>
            <?php
                //Jos tuote löytyi niin näytetään sen speksit
                $stock;
                if ($product['stock'] > 0) {
                    $stock=$product['stock'];
                }
                else {
                    $stock='Out of stock!';
                }

                if(isset($product) && $product['visible'] == true) {
                    echo "Name: " . $product['product_name'] . "<br>" .
                    "Price: " . $product['sellprice'] . "€<br>" .
                    "Stock: " . $stock . "<br>" .
                    "Product Id: " . $product['product_id'] . "<br><br>" .
                    "Description:<br> " . $product['product_desc'] . "<br>";
                }
                if ($product['stock'] > 0) {
                    echo '<form action="mycart.php" method="post">
                    <input style="display: none;" value="'. $product['product_id'] . '" name="product" readonly>
                    Quantity: <input type="number" name="quantity" min="1" max="' . $product['stock'] . '" step="1" value="1" required>
                    <input type="submit" value="Add To Cart">
                    </form><br>
                    <a href="index.php">RETURN TO PRODUCT SEARCH</a>';   
                }
            ?>
        </p>
        <?php
        include 'footer.php';
    ?>
    </body>
</html>