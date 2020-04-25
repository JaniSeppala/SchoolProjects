<?php
    include '../config.php';
    $products;
    $supplier;
    if (isset($_GET['supplier'])) {
        include '../database/database_connections.php';
        $products = db_list_supplier_products($_GET['supplier']);
        $supplier = db_get_supplier_name($_GET['supplier']);

    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>
        <?php echo $store['name']?> - 
            <?php
                if(isset($supplier)) {
                    echo 'Products supplied by ' . $supplier;
                }
                else {
                    echo "Supplier not found";
                }
            ?>
        </title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
            <?php
                echo '<a href="suppliers.php">Return to supplier page</a><br>';
                //Jos toimittaja löytyy niin tarkistetaan onko toimittaja merkitty mihinkään tuotteeseen toimittajaksi ja mikäli on niin tulostetaan tuotteet taulukkoon
                if(isset($supplier)) {
                    echo '<h1>Products supplied by ' . $supplier . ':</h1>';

                    if (isset($products)) {
                        echo '<table border="1">
                        <tr>
                        <td><strong>VISIBLE</strong></td>
                        <td><strong>PRODUCT ID</strong></td>
                        <td><strong>NAME</strong></td>
                        <td><strong>SELLING PRICE</strong></td>
                        <td><strong>STOCK</strong></td>
                        <td><strong>MORE INFO</strong></td>
                        <td><strong>EDIT PRODUCT</strong></td>
                        </tr>';
                        foreach ($products as $row) {
                            $visible;
                            if ($row['visible'] == true) {
                                $visible = 'YES';
                            }
                            else {
                                $visible = 'NO';
                            }
                            $products_url = 'inspect_product.php?product=' . $row['product_id'];
                            $edit_url = 'edit_product.php?product=' . $row['product_id'];
                            echo "<tr>
                            <td>" . $visible . "</td>
                            <td>" . $row['product_id'] . "</td>
                            <td>" . $row['product_name'] . "</td>
                            <td>" . $row['sellprice'] . "€</td>
                            <td>" . $row['stock'] . '</td>
                            <td><a href="' . $products_url .'">INSPECT PRODUCT</a></td>
                            <td><a href="' . $edit_url .'">EDIT PRODUCT</a></td></tr>';
                        }

                        echo '</table>';
                    }
                    //Jos ei niin ilmoitetaan että toimittaja ei ole vielä minkään tuotteen toimittaja
                    else {
                        echo 'No products supplied by '. $supplier . ' found.';
                    }
                }
                //Jos toimittajaa ei löydy niin ilmoitetaan siitä käyttäjälle
                else {
                    echo "The supplier was not found";
                }
                
            ?>
        <p>
            <?php
                //Jos tuote löytyi niin näytetään sen speksit
                if(isset($product) && $product['visible'] == true) {
                    echo "Name: " . $product['product_name'] . "<br>" .
                    "Price: " . $product['sellprice'] . "€<br>" .
                    "Stock: " . $product['stock'] . "<br>" .
                    "Product Id: " . $product['product_id'] . "<br><br>" .
                    "Description:<br> " . $product['product_desc'] . "<br><br>";
                }
            ?>
        </p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>