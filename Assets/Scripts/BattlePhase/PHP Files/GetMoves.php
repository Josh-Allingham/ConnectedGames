<?php
    $sql_connect = mysqli_connect("stateri-elementi.chmiq4ey60h8.eu-north-1.rds.amazonaws.com", "admin", "Element_Game26") or die("No DB Connections");
    mysqli_select_db($sql_connect, "stateri_elementi") or die("DB not found");

    $query = "SELECT * FROM moves";

    $result = mysqli_query($sql_connect, $query) or die("Query Failed");

    $num_results = mysqli_num_rows($result);

    for ($i = 0; $i < $num_results; $i++) {
        $row = mysqli_fetch_array($result);
        echo $row['Element'] . "," . $row['Move_Name'] . "," . $row['Power'] . "," . $row['Accuracy'] . "," . $row['Target'] . "," . $row['Description'] . "," . $row['Turns'] . ",";
    }

    mysqli_close($sql_connect);
?>