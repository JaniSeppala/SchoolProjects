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
        <title><?php echo $store['name']?> - Users</title>
    </head>
    <body>
    <?php
        include 'navigation.php';
    ?>
        <h1>Users</h1>
        <?php
            if ($_SESSION['superuser_users']) {
                echo '<p><a href="add_user.php">ADD A USER</a></p>';
            }
            $users = db_list_users();
            //Jos käyttäjiä löytyy niin tulostetaan ne taulukkoon
            if (isset($users)) {
                echo '<table border="1">
                <tr>
                <td><strong>SUPER USER</strong></td>
                <td><strong>USERNAME</strong></td>
                <td><strong>FIRST NAME</strong></td>
                <td><strong>LAST NAME</strong></td>
                <td><strong>MORE INFO</strong></td>';
                if ($_SESSION['superuser_users']) {
                    echo '<td><strong>EDIT USER</strong></td>';
                }
                echo '</tr>';
                foreach ($users as $row) {
                    $superuser;
                    if ($row['superuser'] == true) {
                        $superuser = 'YES';
                    }
                    else {
                        $superuser = 'NO';
                    }
                    $inspect_url = 'inspect_user.php?user=' . $row['id'];
                    $edit_url = 'edit_user.php?user=' . $row['id'];
                    echo "<tr>
                    <td>" . $superuser . "</td>
                    <td>" . $row['username'] . "</td>
                    <td>" . $row['first_name'] . "</td>
                    <td>" . $row['last_name'] . '</td>
                    <td><a href="' . $inspect_url .'">INSPECT USER</a></td>';
                    if ($_SESSION['superuser_users']) {
                        echo '<td><a href="' . $edit_url .'">EDIT USER</a></td>';
                    }
                    echo '</tr>';
                }

                echo '</table>';
            }
            //Jos käyttäjiä ei löydy niin ilmoitetaan siitä
            else {
                echo "No users found";
            }
            
        ?>
    <?php
        include '../footer.php';
    ?>
    </body>
</html>