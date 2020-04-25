<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();
    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if ($_SESSION['superuser_products']) {
            $suppliers = db_list_suppliers();
        }
        else {
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
        <title><?php echo $store['name']?> - Add Product</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>ADD A PRODUCT</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            Product ID:<br><input style="width:220px;" type="text" name="productid" maxlength="30" placeholder="MAX 30 CHARACTERS" required><br>
            Name:<br><input style="width:360px;" type="text" name="name" maxlength="50" placeholder="MAX 50 CHARACTERS" required><br>
            Stock:<br><input type="number" name="stock" min="0" max="999999999" step="1" required><br>
            Selling Price:<br><input type="number" name="sellprice" min="0" max="999999999.99" step=".01" required><br>
            Buying Price:<br><input type="number" name="buyprice" min="0" max="999999999.99" step=".01" required><br>
            EAN Code:<br><input type="text" name="ean" maxlength="30" placeholder="MAX 30 CHARACTERS" required><br>
            <input type="checkbox" name="visible" value="yes" checked>Visible<br>
            Supplier:<br><select name="supplier">
                <option value="null">NONE</option>
                <?php
                    //Tulostetaan kaikki tavarantoimittajat listaan
                    if (isset($suppliers)) {
                        foreach ($suppliers as $row) {
                            echo '<option value="' . $row['id'] . '">' . $row['supplier_name'] . '</option>';
                        }
                    }
                ?>
            </select><br> 
            Suppliers Product ID:<br><input type="text" name="supplierproductid" maxlength="30" placeholder="MAX 30 CHARACTERS"><br>
            Description:<br><textarea style="width:400px; height:180px;" name="description" maxlength="500" placeholder="MAX 500 CHARACTERS"></textarea><br>
            <input type="submit" value="Add">
        </form><br>
        <?php
            //Jos lomake on täytetty ja lisäys-painiketta painettu niin lisätään tiedot tietokantaan
            if (isset($_POST['productid'])) {
                $product = $_POST;//Kopioidaan POST-muuttujan tiedot uuteen muuttujaan
                if (db_add_product($product)) {
                    echo 'PRODUCT ADDED SUCCESSFULLY!';
                }
                else {
                    echo 'Failed to add a product!';
                }
            }
        ?>
        <p><a href="products.php">Return to products page</a></p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>