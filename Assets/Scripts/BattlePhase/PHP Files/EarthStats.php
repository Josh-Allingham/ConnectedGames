<?php
	$sql_connect = mysqli_connect("localhost","root", "") or die ("No DB Connections");
	mysqli_select_db($sql_connect,"stateri_elementi") or die ("DB not found");

	$query = "SELECT * FROM player_stats WHERE Element_Type = 'Earth'";

	$result = mysqli_query($sql_connect,$query) or die ("Query Failed");

	$num_results = mysqli_num_rows ($result);

	for($i = 0; $i < $num_results; $i++)
	{
		$row = mysqli_fetch_array($result);
		echo $row['Curr_Health'] . "," . $row['Max_Health'] . "," . $row['Defence'] . "," . $row['Attack'] .  "," . $row['Speed'] . "," .$row['Elemental_Statera'];
	}

	mysqli_close($sql_connect);
?>