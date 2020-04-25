<?php
    session_start();
    include 'config.php';
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Product Search</title>
    </head>
    <body>
        <?php
            include 'navigation.php';
        ?>
        <h1>SEARCH PRODUCTS</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            SEARCH:<input type="search" name="searchproduct" required>
            <input type="submit" value="Search"><br>
        </form>
        <?php
            //Tsekataan onko käyttäjä tehnyt hakua
            if (isset($_POST['searchproduct'])) {
                include 'database/database_connections.php';
                $results = db_search_products($_POST['searchproduct']);
                $numberofresults = 0;
                //Jos saadaan tuloksia niin tulostetaan ne käyttäjälle taulukkoon ja jos ei, niin näytetään kaikki tuotteet joita ei ole piilotettu
                if (isset($results)) {
                    foreach ($results as $row) {
                        //Näytetään vain tuotteet, joita ei ole piilotettu
                        if ($row['visible'] == true) {
                            $numberofresults = $numberofresults + 1;
                            //Tämä ajetaan vain kun ensimmäinen tulos näytetään
                            if ($numberofresults == 1) {
                                echo 'RESULTS:<br><table border="1">
                                <tr>
                                <td><strong>NAME</strong></td>
                                <td><strong>PRICE</strong></td>
                                <td><strong>STOCK</strong></td>
                                <td><strong>ADD TO CART</strong></td>
                                </tr>';
                            }
                            $url = 'inspect_product.php?product=' . $row['product_id'];
                            echo '<tr>
                            <td><a href="' . $url . '">' . $row['product_name'] . "</a></td>
                            <td>" . $row['sellprice'] . "€</td>
                            <td>" . $row['stock'] . '</td>
                            <td><form action="mycart.php" method="post">
                            <input style="display: none;" value="'. $row['product_id'] . '" name="product" readonly>
                            Quantity: <input type="number" name="quantity" min="1" max="' . $row['stock'] . '" step="1" value="1" required>
                            <input type="submit" value="Add To Cart">
                            </form></td></tr>';
                        }
                    }
                    echo '</table>';
                }

                if (!isset($results) || $numberofresults == 0){
                    echo 'NO RESULTS';
                }
            }
        ?>
    <?php
        include 'footer.php';
    ?>
    </body>
</html>