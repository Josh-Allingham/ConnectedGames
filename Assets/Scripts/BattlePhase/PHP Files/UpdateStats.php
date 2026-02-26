<?php
	$servername = "localhost";
	$username = "root";
	$password = "";
	$dbname = "stateri_elementi";

	$Element_Type = $_POST['Element_Type'];
	$Curr_Health = $_POST['Curr_Health'];
	$Max_Health = $_POST['Max_Health'];
	$Defence = $_POST['Defence'];
	$Attack = $_POST['Attack'];
	$Speed = $_POST['Speed'];
	$Elemental_Statera = $_POST['Elemental_Statera'];

	$conn = new mysqli($servername, $username, $password, $dbname);

	if($conn->connect_error)
	{
		die("connection failed: " . $conn->connect_error);
	}

	$sql = "UPDATE player_stats SET Curr_Health = " .$Curr_Health . ", Max_Health = " . $Max_Health . ", Defence = " . $Defence . ", Attack = " . $Attack . ", Speed = " . $Speed . ", Elemental_Statera = " . $Elemental_Statera . " WHERE Element_Type = '" . $Element_Type . "'";

	if($conn->query($sql) === TRUE)
	{
		echo "Record updated successfully";
	}
	else
	{
		echo "Error updating record: " . $sql . "<br>". $conn->error;
	}

	$conn->close();
?>