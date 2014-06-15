<?php
require("sql.php");
session_start();

function login($request) {
	global $db;
	
	$qry = "SELECT * FROM users WHERE user = ?";
	if($stmt = $db->prepare($qry)) {
		$stmt->bind_param("s", $request['user']);
		$stmt->execute();
		$result = $stmt->get_result();
		while($row = $result->fetch_assoc())
		{
			$salt = $row['salt'];
			$hashedpw = hash('sha256', $request['pass'] . $salt);
			if($row['pass'] === $hashedpw) {
				return json_encode("5544332211SessionID");
			} else {
				return json_encode("LOGIN_FAILED");
			}
		}
		return json_encode("LOGIN_FAILED");
	}
}

function register($username, $pass) {
	global $db;
	$qry = "INSERT INTO users (user, pass, salt) VALUES (?,?,?);";
	if($stmt = $db->prepare($qry)) {
		$salt = bin2hex(openssl_random_pseudo_bytes(3));
		$hashedpw = hash('sha256', $pass . $salt);
		$stmt->bind_param("sss", $username, $hashedpw, $salt);
		$stmt->execute();
		return json_encode("REGISTER_SUCCESS");
	}
	return json_encode("REGISTER_FAILED");	

}

//Global gamestate object
$gamestate = array();

function generate_gamestate() {
	global $gamestate;
	$gamestate = array(
		//Initial game state array (SolReg positions etc). This should be generated/made persistent.
		array(
			//sector id, planet id, xpos, zpos, scale, texture, owner, income, slots, emptyslots, array(connections), array(buildings)
			array(0,0,15.6,2.9,0.5,1,0,10,3,0,array(1, 2), array()),
			array(0,1,1.9,-8.5,0.2,4,0,14,2,0,array(0, 3), array()),
			array(0,2,-8.2,-4.9,0.6,2,0,18,4,0,array(0), array()),
			array(0,3,9.5,-6.7,0.1,3,0,4,3,0,array(1), array()),
			array(0,4,-0.3,-1.24,0.3,3,0,7,5,0,array(5), array()),
			array(0,5,-4,-14.3,0.7,1,0,24,7,0,array(6, 4), array()),
			array(0,6,16,16.5,0.8,4,0,14,1,0,array(7, 5), array()),
			array(0,7,9.6,14.36,0.8,3,0,29,2,0,array(8, 6), array()),
			array(0,8,6.6,3.92,0.4,3,0,3,2,0,array(9, 7), array()),
			array(0,9,-14.56,-14.7,0.5,1,0,11,4,0,array(8), array())
		)
	);	
}

/*
Return the game-state to the player. 
This should only be called upon player connection.
*/

function get_gamestate($request) {
	global $gamestate;
	echo json_encode($gamestate);
}

function player_event($request) {

}
