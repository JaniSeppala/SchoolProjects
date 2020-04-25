<?php

    function db_add_order(Array $order, Array $rows) {
        include 'config.php';//Haetaan conffit
        $userid = 'null';
        if (isset($order['user_id'])) {
            $userid = $order['user_id'];
        }
        $firstname = $order['firstname'];
        $lastname = $order['lastname'];
        $address = $order['address'];
        $postalcode = $order['postalcode'];
        $city = $order['city'];
        $email = $order['email'];
        $phone = 'null';
        if (isset($order['phone'])) {
            $phone = $order['phone'];
        }
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Order';//Asetetaan taulun nimi
        //Ensin luodaan tilaus tietokantaan
        $query = mysqli_query($connection, "INSERT INTO $table(user_id, order_datetime, delivery_firstname, delivery_lastname, delivery_address, delivery_postalcode, delivery_city, delivery_email, delivery_phone)
            VALUES ($userid, NOW(), '$firstname', '$lastname', '$address', '$postalcode', '$city', '$email', '$phone')");//Ajetaan kysely
        //Haetaan äsken lisätyn tilauksen id LAST_INSERT_ID funktion avulla ja sen jälkeen asetetaan uusi taulun nimi table-muuttujaan
        $table2 = $database['table_prefix'] . 'OrderRow';
        $table3= $database['table_prefix'] . 'Product';
        $query = mysqli_query($connection, "SELECT LAST_INSERT_ID() AS id");
        $order_id;
        if (mysqli_num_rows($query)) {
            $result_array = mysqli_fetch_assoc($query);
            $order_id = $result_array['id'];
        }
        //Sitten tallennetaan jokainen tilauksen rivi tietokantaan ja vähennetään saldoa jokaiselta tuotteelta tilatun määrän verran
        foreach ($rows as $row) {
            $id = $row['id'];
            $quantity = $row['quantity'];
            $query = mysqli_query($connection, "UPDATE $table3 SET stock = stock-$quantity WHERE id=$id");
            $query = mysqli_query($connection, "INSERT INTO $table2(order_id,product_id,quantity) VALUES ($order_id, $id, $quantity)");//Ajetaan kysely
            echo 'ERROR MESSAGE: ' . mysqli_error($connection) . '<br>';
        }
    }

    function db_add_product(Array $product) {
        include 'config.php';//Haetaan conffit
        $product_id = $product['productid'];
        $name = $product['name'];
        $stock = $product['stock'];
        $sellprice = $product['sellprice'];
        $buyprice = $product['buyprice'];
        $ean = $product['ean'];
        $supplier = $product['supplier'];
        $Supplier_product_id = $product['supplierproductid'];
        $visible;

        if (isset($product['visible'])) {
            $visible = 'true';
        }
        else {
            $visible = 'false';
        }

        $description = $product['description'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "INSERT INTO $table(product_id,product_name,product_desc,sellprice,buyprice,ean_code,visible,stock,supplier,supplier_product_id)
            VALUES ('$product_id','$name','$description',$sellprice,$buyprice,'$ean',$visible,$stock,$supplier,'$Supplier_product_id')");//Ajetaan kysely
        return true;
    }

    function db_add_supplier(Array $supplier) {
        include 'config.php';//Haetaan conffit
        $name = $supplier['name'];
        $address = $supplier['address'];
        $postalcode = $supplier['postalcode'];
        $city = $supplier['city'];
        $email = $supplier['email'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Supplier';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "INSERT INTO $table(supplier_name,supplier_address,supplier_postalcode,supplier_city,supplier_email)
            VALUES ('$name','$address','$postalcode','$city','$email')");//Ajetaan kysely
        return true;
    }

    function db_add_user(Array $user) {
        include 'config.php';//Haetaan conffit
        $username = $user['username'];
        $password = password_hash($user['password'], PASSWORD_DEFAULT);//Salataan salasana
        $firstname = $user['firstname'];
        $lastname = $user['lastname'];
        $address = $user['address'];
        $postalcode = $user['postalcode'];
        $city = $user['city'];
        $email = $user['email'];
        $phone = $user['phone'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        mysqli_query($connection, "INSERT INTO $table(username,userpassword,first_name,last_name,user_address,user_postalcode,user_city,user_email,user_phone)
            VALUES ('$username','$password','$firstname','$lastname','$address','$postalcode','$city','$email','$phone')");//Ajetaan kysely
        return true;
    }

    function db_add_user_admin(Array $user) {
        include 'config.php';//Haetaan conffit
        $username = $user['username'];
        $password = password_hash($user['password'], PASSWORD_DEFAULT);//Salataan salasana
        $firstname = $user['firstname'];
        $lastname = $user['lastname'];
        $address = $user['address'];
        $postalcode = $user['postalcode'];
        $city = $user['city'];
        $email = $user['email'];
        $phone = $user['phone'];
        $superuser = 'false';
        $superuser_products = 'false';
        $superuser_suppliers = 'false';
        $superuser_orders = 'false';
        $superuser_users = 'false';

        if (isset($user['superuser'])) {
            $superuser='true';
        }

        if (isset($user['superuser_products'])) {
            $superuser_products='true';
        }

        if (isset($user['superuser_suppliers'])) {
            $superuser_suppliers='true';
        }

        if (isset($user['superuser_orders'])) {
            $superuser_orders='true';
        }

        if (isset($user['superuser_users'])) {
            $superuser_users='true';
        }

        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        mysqli_query($connection, "INSERT INTO $table(username,userpassword,first_name,last_name,user_address,user_postalcode,user_city,user_email,user_phone,
            superuser,superuser_products,superuser_suppliers,superuser_orders,superuser_users)
            VALUES ('$username','$password','$firstname','$lastname','$address','$postalcode','$city','$email','$phone',$superuser,$superuser_products,$superuser_suppliers,$superuser_orders,$superuser_users)");//Ajetaan kysely
        return true;
    }

    function db_connect() {
        include 'config.php';//Haetaan conffit
        $connection = mysqli_connect($database['ip'], $database['user'], $database['password'], $database['name']);//Yritetään muodostaa yhteys tietokantaan
        //Jos yhdistäminen ei onnistu niin tulostetaan virheilmoitus ja jos onnistutaan niin palautetaan yhteys kutsuvalle funktiolle
        if (mysqli_connect_errno($connection)) {
            echo "Failed to connect to MySQL: " . mysqli_connect_error();
        }
        else {
            //Varmistetaan että kaikki merkit, kuten ääkköset, toimivat oikein ja palautetaan yhteys kutsuvalle funktiolle
            mysqli_set_charset($connection, "utf8");
            return $connection;
        }
    }

    function db_delete_order(int $orderid) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'OrderRow';//Asetetaan taulun nimi
        //Ensin haetaan kaikki tilatut tuotteet ja niiden tilausmäärät
        $result = mysqli_query($connection, "SELECT product_id, quantity FROM $table WHERE order_id=$orderid");
        $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
        //Sitten lisätään jokaisen tilatun tuotteen saldoa tilausmäärällä
        $table = $database['table_prefix'] . 'Product';
        foreach ($result_array as $row) {
            $quantity = $row['quantity'];
            $id=$row['product_id'];
            $result = mysqli_query($connection, "UPDATE $table SET stock=stock+$quantity WHERE id=$id");
        }
        //Sitten poistetaan kaikki tilauksen rivit
        $table = $database['table_prefix'] . 'OrderRow';
        $result = mysqli_query($connection, "DELETE FROM $table WHERE order_id=$orderid");
        //Ja lopuksi poistetaan itse tilaus
        $table = $database['table_prefix'] . 'Order';
        $result = mysqli_query($connection, "DELETE FROM $table WHERE id=$orderid");
    }

    function db_edit_product(Array $product) {
        include 'config.php';//Haetaan conffit
        $id = $product['id'];
        $product_id = $product['productid'];
        $name = $product['name'];
        $stock = $product['stock'];
        $sellprice = $product['sellprice'];
        $buyprice = $product['buyprice'];
        $ean = $product['ean'];
        $supplier = $product['supplier'];
        $supplier_product_id = $product['supplierproductid'];
        $visible;

        if (isset($product['visible'])) {
            $visible = 'true';
        }
        else {
            $visible = 'false';
        }

        $description = $product['description'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "UPDATE $table SET product_name='$name', product_desc='$description', sellprice=$sellprice, buyprice=$buyprice, ean_code='$ean', visible=$visible, stock=$stock, supplier=$supplier, supplier_product_id='$supplier_product_id' WHERE id = $id");//Ajetaan kysely
        return true;
    }

    function db_edit_supplier(Array $supplier) {
        include 'config.php';//Haetaan conffit
        $id = $supplier['id'];
        $name = $supplier['name'];
        $address = $supplier['address'];
        $postalcode = $supplier['postalcode'];
        $city = $supplier['city'];
        $email = $supplier['email'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Supplier';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "UPDATE $table SET supplier_name='$name', supplier_address='$address', supplier_postalcode='$postalcode', supplier_city='$city', supplier_email='$email' WHERE id = $id");//Ajetaan kysely
        return true;
    }

    function db_edit_user(Array $user) {
        include 'config.php';//Haetaan conffit
        $id = $user['id'];
        $username = $user['username'];
        $firstname = $user['firstname'];
        $lastname = $user['lastname'];
        $address = $user['address'];
        $postalcode = $user['postalcode'];
        $city = $user['city'];
        $email = $user['email'];
        $phone = $user['phone'];
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "UPDATE $table SET username='$username', first_name='$firstname', last_name='$lastname', user_address='$address', user_postalcode='$postalcode', user_city='$city', user_email='$email', user_phone='$phone' WHERE id = $id");//Ajetaan kysely
        return true;
    }

    function db_edit_user_admin(Array $user) {
        include 'config.php';//Haetaan conffit
        $id = $user['id'];
        $username = $user['username'];
        $firstname = $user['firstname'];
        $lastname = $user['lastname'];
        $address = $user['address'];
        $postalcode = $user['postalcode'];
        $city = $user['city'];
        $email = $user['email'];
        $phone = $user['phone'];
        $superuser = 'false';
        $superuser_products = 'false';
        $superuser_suppliers = 'false';
        $superuser_orders = 'false';
        $superuser_users = 'false';

        if (isset($user['superuser'])) {
            $superuser='true';
        }

        if (isset($user['superuser_products'])) {
            $superuser_products='true';
        }

        if (isset($user['superuser_suppliers'])) {
            $superuser_suppliers='true';
        }

        if (isset($user['superuser_orders'])) {
            $superuser_orders='true';
        }

        if (isset($user['superuser_users'])) {
            $superuser_users='true';
        }

        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "UPDATE $table SET username='$username', first_name='$firstname', last_name='$lastname', user_address='$address', user_postalcode='$postalcode', user_city='$city', user_email='$email', user_phone='$phone',
            superuser=$superuser, superuser_products=$superuser_products, superuser_suppliers=$superuser_suppliers, superuser_orders=$superuser_orders, superuser_users=$superuser_users WHERE id = $id");//Ajetaan kysely
        return true;
    }

    function db_get_supplier_name(int $supplier_id) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Supplier';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT supplier_name FROM $table WHERE id = $supplier_id");//Ajetaan kysely
        //Palautetaan tulos jo jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array['supplier_name'];
        }
    }

    function db_get_order_rows(int $order) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        //Asetetaan taulujen nimet
        $table = $database['table_prefix'] . 'OrderRow';
        $table2 = $database['table_prefix'] . 'Product';
        $result = mysqli_query($connection, "SELECT $table.*, product_name, sellprice FROM $table INNER JOIN $table2 ON $table2.id=$table.product_id WHERE order_id = $order");//Ajetaan kysely
        //Palautetaan tulos jo jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_inspect_order(int $orderid, int $userid) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Order';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE id = $orderid AND user_id=$userid");//Ajetaan kysely
        //Palautetaan tulos jos jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array;
        }
    }

    function db_inspect_order_admin(int $orderid) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Order';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE id = $orderid");//Ajetaan kysely
        //Palautetaan tulos jos jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array;
        }
    }

    function db_inspect_product($product_id) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $escaped_query = mysqli_real_escape_string ($connection , $product_id);//Poistetaan kyselystä erikoismerkit
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE product_id = '$escaped_query'");//Ajetaan kysely
        //Palautetaan tulos jos jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array;
        }
    }

    function db_inspect_supplier($supplier) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Supplier';//Asetetaan taulun nimi
        $escaped_query = mysqli_real_escape_string ($connection , $supplier);//Poistetaan kyselystä erikoismerkit
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE id = $escaped_query");//Ajetaan kysely
        //Palautetaan tulos jos jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array;
        }
    }

    function db_inspect_user($user) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $escaped_query = mysqli_real_escape_string ($connection , $user);//Poistetaan kyselystä erikoismerkit
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE id = $escaped_query");//Ajetaan kysely
        //Palautetaan tulos jos jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_assoc($result);
            return $result_array;
        }
    }

    function db_list_orders() {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Order';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table ORDER BY order_datetime DESC");
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_list_products() {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table ORDER BY product_id");
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_list_suppliers() {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Supplier';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table ORDER BY supplier_name");
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_list_users() {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table ORDER BY username");
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_list_users_orders(int $userid) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Order';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table where user_id=$userid ORDER BY order_datetime DESC");
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_list_supplier_products(int $supplier_id) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE supplier = $supplier_id");//Ajetaan kysely
        //Palautetaan tulos jo jotain löytyy
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_search_products($query) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'Product';//Asetetaan taulun nimi
        $escaped_query = '%' . mysqli_real_escape_string ($connection , $query) . '%';//Poistetaan kyselystä erikoismerkit
        $result = mysqli_query($connection, "SELECT * FROM $table WHERE product_name LIKE '$escaped_query' OR product_desc LIKE '$escaped_query'");//Ajetaan kysely
        //Jos kysely palautti tuloksia niin talletetaan tulokset taulukkoon ja palautetaan taulukko kutsuvalle funktiolle
        if (mysqli_num_rows($result) > 0) {
            $result_array = mysqli_fetch_all($result, MYSQLI_ASSOC);
            return $result_array;
        }
    }

    function db_user_exists(String $username) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "SELECT username FROM $table WHERE username='$username'");
        
        if (mysqli_num_rows($query) > 0) {
            return true;
        }
        else {
            return false;
        }
    }

    function db_validate_user(String $username, String $password) {
        include 'config.php';//Haetaan conffit
        $connection = db_connect();//Luodaan yhteys tietokantaan
        $table = $database['table_prefix'] . 'User';//Asetetaan taulun nimi
        $query = mysqli_query($connection, "SELECT * FROM $table WHERE username='$username'");//Ajetaan kysely
        //Palautetaan tulos jo jotain löytyy
        if (mysqli_num_rows($query) > 0) {
            $user = mysqli_fetch_assoc($query);
            if (password_verify($password, $user['userpassword'])) {
                return $user;
            }
        }
    }

?>