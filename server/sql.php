<?php

$db = new mysqli("localhost", "galaxy", "galaxy123", "g_supremacy");
if(mysqli_connect_error()) {
	die("Database Error (" . mysqli_connect_errno() . ") " . mysqli_connect_error());
}

