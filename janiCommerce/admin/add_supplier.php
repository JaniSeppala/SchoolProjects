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
?>
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title><?php echo $store['name']?> - Add Supplier</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>ADD A SUPPLIER</h1>
        <form action="<?php echo $_SERVER['PHP_SELF'];?>" method="post">
            Name:<br><input type="text" name="name" maxlength="50" placeholder="MAX 50 CHARACTERS" required><br>
            Address:<br><input type="text" name="address" maxlength="50" placeholder="MAX 50 CHARACTERS"><br>
            Postal Code:<br><input type="text" name="postalcode" maxlength="10" placeholder="MAX 10 CHARACTERS"><br>
            City:<br><input type="text" name="city" maxlength="40" placeholder="MAX 40 CHARACTERS"><br>
            Email:<br><input type="email" name="email" maxlength="80" placeholder="MAX 80 CHARACTERS"><br>
            <input type="submit" value="Add">
        </form><br>
        <?php
            //Jos lomake on täytetty ja lisäys-painiketta painettu niin lisätään tiedot tietokantaan
            if (isset($_POST['name'])) {
                $supplier = $_POST;//Kopioidaan POST-muuttujan tiedot uuteen muuttujaan
                if (db_add_supplier($supplier)) {
                    echo 'Supplier added successfully!';
                }
                else {
                    echo 'Failed to add a supplier!';
                }
            }
        ?>
    <?php
        include '../footer.php';
    ?>
        <p><a href="suppliers.php">RETURN TO SUPPLIERS PAGE</a></p>
    </body>
</html>