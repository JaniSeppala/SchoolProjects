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
        <title><?php echo $store['name']?> - Suppliers</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Suppliers</h1>
        <?php
            if ($_SESSION['superuser_suppliers']) {
                echo '<p><a href="add_supplier.php">ADD A SUPPLIER</a></p>';
            }
            $results = db_list_suppliers();
            //Jos saadaan tuloksia niin tulostetaan ne käyttäjälle taulukkoon ja jos ei, niin kerrotaan että tuloksia ei löytynyt
            if (isset($results)) {
                echo 'SUPPLIERS:<br><table border="1">
                <tr>
                <td><strong>NAME</strong></td>
                <td><strong>ADDRESS</strong></td>
                <td><strong>POSTAL CODE</strong></td>
                <td><strong>CITY</strong></td>
                <td><strong>EMAIL</strong></td>
                <td><strong>SHOW PRODUCTS</strong></td>';
                if ($_SESSION['superuser_suppliers']) {
                    echo '<td><strong>EDIT SUPPLIER</strong></td>';
                }
                echo '</tr>';

                foreach ($results as $row) {
                    $products_url = 'supplier_products.php?supplier=' . $row['id'];
                    $edit_url = 'edit_supplier.php?supplier=' . $row['id'];
                    echo "<tr>
                    <td>" . $row['supplier_name'] . "</td>
                    <td>" . $row['supplier_address'] . "</td>
                    <td>" . $row['supplier_postalcode'] . "</td>
                    <td>" . $row['supplier_city'] . "</td>
                    <td>" . $row['supplier_email'] . '</td>
                    <td><a href="' . $products_url .'">SHOW SUPPLIERS PRODUCTS</a></td>';
                    if ($_SESSION['superuser_suppliers']) {
                        echo '<td><a href="' . $edit_url .'">EDIT SUPPLIER</a></td>';
                    }
                    echo '</tr>';
                }
                echo '</table>';
            }
            else {
                echo 'NO SUPPLIERS FOUND';
            }
        ?>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>