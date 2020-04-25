<?php
    include '../database/database_connections.php';
    include '../config.php';
    session_start();

    if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
        if (!$_SESSION['superuser_suppliers']) {
            header("Location: ../myhome.php");
            die();
        }
    }
    else {
        header("Location: ../login.php");
        die();
    }

    $supplier;
    $success;

    if (isset($_GET['supplier'])) {
        $supplier = db_inspect_supplier($_GET['supplier']);
    }
    else if (isset($_POST['id'])) {
        $supplier = $_POST;//Kopioidaan POST-muuttujan tiedot muuttujaan
        if (db_edit_supplier($supplier)) {
            $success = true;
        }
        else {
            $success = false;
        }

        $supplier = db_inspect_supplier($_POST['id']);
    }

    if (!isset($supplier)) {
        header("Location: ../index.php");
        die();
    }
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Editing <?php echo $supplier['supplier_name'];?></title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Edit <?php echo $supplier['supplier_name'];?></h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            <input style="display: none;" type='text' name="id" value="<?php echo $supplier['id'];?>" readonly>
            Name:<br><input type="text" name="name" maxlength="50" value="<?php echo $supplier['supplier_name'];?>" required><br>
            Address:<br><input type="text" name="address" maxlength="50" value="<?php echo $supplier['supplier_address'];?>"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" value="<?php echo $supplier['supplier_postalcode'];?>"><br>
            City:<br><input type="text" name="city" maxlength="40" value="<?php echo $supplier['supplier_city'];?>"><br>
            Email:<br><input type="email" name="email" maxlength="80" value="<?php echo $supplier['supplier_email'];?>"><br>
            <input type="submit" value="Save Changes">
        </form>
        <?php
            //Ilmoitetaan käyttäjälle onnistuiko tietojen päivitys
            if (isset($success)) {
                echo '<br>';
                if ($success) {
                    echo 'Supplier info updated successfully!';
                }
                else {
                    echo 'Failed to update supplier!';
                }
            }
        ?>
        <p><a href="suppliers.php">RETURN TO SUPPLIERS PAGE</a></p>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>