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
        <title><?php echo $store['name']?> - Products</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Products</h1>
        <?php
            if ($_SESSION['superuser_products']) {
                echo '<p><a href="add_product.php">ADD A PRODUCT</a></p>';
            }
            $products = db_list_products();
            //Jos tuotteita löytyy niin tulostetaan tuotteet taulukkoon
            if (isset($products)) {
                echo '<table border="1">
                <tr>
                <td><strong>VISIBLE</strong></td>
                <td><strong>PRODUCT ID</strong></td>
                <td><strong>NAME</strong></td>
                <td><strong>SELLING PRICE</strong></td>
                <td><strong>STOCK</strong></td>
                <td><strong>MORE INFO</strong></td>';
                if ($_SESSION['superuser_products']) {
                    echo '<td><strong>EDIT PRODUCT</strong></td>';
                }
                echo '</tr>';
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
                    <td><a href="' . $products_url .'">INSPECT PRODUCT</a></td>';
                    if ($_SESSION['superuser_products']) {
                        echo '<td><a href="' . $edit_url .'">EDIT PRODUCT</a></td>';
                    }
                    echo '</tr>';
                }

                echo '</table>';
            }
            //Jos tuotteita ei löydy niin ilmoitetaan siitä käyttäjälle
            else {
                echo "No products found";
            }
            
        ?>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>