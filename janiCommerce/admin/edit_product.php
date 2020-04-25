<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();

    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser_products']) {
            header("Location: ../myhome.php");
            die();
        }
    }
    else {
        header("Location: ../login.php");
        die();
    }

    $product;
    $success;
    $visible = "";
    if (isset($_GET['product'])) {
        $product = db_inspect_product($_GET['product']);
    }
    else if (isset($_POST['id'])) {
        $product = $_POST;//Kopioidaan POST-muuttujan tiedot muuttujaan
        if (db_edit_product($product)) {
            $success = true;
        }
        else {
            $success = false;
        }

        $product = db_inspect_product($_POST['productid']);
    }

    if (!isset($product)) {
        header("Location: ../index.php");
        die();
    }

    $suppliers = db_list_suppliers();
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Editing <?php echo $product['product_name'];?></title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Edit <?php echo $product['product_name'];?></h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            <input style="display: none;" type="text" name="id" value="<?php echo $product['id'];?>" readonly>
            Product ID(Read only):<br><input style="width:220px;" type="text" name="productid" maxlength="30" value="<?php echo $product['product_id'];?>" readonly><br>
            Name:<br><input style="width:360px;" type="text" name="name" maxlength="50" value="<?php echo $product['product_name'];?>" required><br>
            Stock:<br><input type="number" name="stock" min="0" max="999999999" step="1" value="<?php echo $product['stock'];?>" required><br>
            Selling Price:<br><input type="number" name="sellprice" min="0" max="999999999.99" step=".01" value="<?php echo $product['sellprice'];?>" required><br>
            Buying Price:<br><input type="number" name="buyprice" min="0" max="999999999.99" step=".01" value="<?php echo $product['buyprice'];?>" required><br>
            EAN Code:<br><input type="text" name="ean" maxlength="30" placeholder="MAX 30 CHARACTERS" value="<?php echo $product['ean_code'];?>" required><br>
            <?php
                if ($product['visible']) {
                    $visible='checked';
                }
            ?>
            <input type="checkbox" name="visible" value="yes" <?php echo $visible;?>>Visible<br>
            Supplier:<br><select name="supplier">
                <option value="null">NONE</option>
                <?php
                    //Tulostetaan kaikki tavarantoimittajat listaan
                    if (isset($suppliers)) {
                        foreach ($suppliers as $row) {
                            if ($row['id'] == $product['supplier']) {
                                echo '<option value="' . $row['id'] . '" selected>' . $row['supplier_name'] . '</option>';
                            }
                            else {
                                echo '<option value="' . $row['id'] . '">' . $row['supplier_name'] . '</option>';
                            }
                        }
                    }
                ?>
            </select><br> 
            Suppliers Product ID:<br><input type="text" name="supplierproductid" maxlength="30" value="<?php echo $product['supplier_product_id'];?>"><br>
            Description:<br><textarea style="width:400px; height:180px;" name="description" maxlength="500"><?php echo $product['product_desc'];?></textarea><br>
            <input type="submit" value="Edit">
        </form>
        <?php
            //Ilmoitetaan käyttäjälle onnistuiko tietojen päivitys
            if (isset($success)) {
                echo '<br>';
                if ($success) {
                    echo 'Product info updated successfully!';
                }
                else {
                    echo 'Failed to update product!';
                }
            }
        ?>
        <p><a href="products.php">RETURN TO PRODUCTS PAGE</a></p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>