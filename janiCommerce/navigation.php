<header>
    <h1><?php echo $store['name']?></h1>

</header>
<div>-------------------------------------------------------------------------------------------------------</div>
<nav>
    <a href="index.php">BACK TO THE STORE</a> | 
    <?php
        if (isset($_SESSION['id']) && isset($_SESSION['password'])) {
            echo '<a href="logout.php">LOG OUT</a> | 
            <a href="myhome.php">MY HOME</a> | ';
        }
        else {
            echo '<a href="login.php">LOG IN</a> | 
            <a href="create_account.php">REGISTER</a> | ';
        }

        echo '<a href="mycart.php">MY SHOPPING CART</a>'
    ?>
</nav>
<div>-------------------------------------------------------------------------------------------------------</div>