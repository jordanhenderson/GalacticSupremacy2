<?php
define("IN_GAME", TRUE);
require("include/game_logic.php");

$ACTION_SEND_CHAT = 0;
$ACTION_GET_CHAT = 1;
$ACTION_GET_GAMESTATE = 2;
$ACTION_PLAYER_EVENT = 3;


$r = json_decode(file_get_contents('php://input'), true);
$action = $r['action'];
$request = $r['data'];

switch($action) {
	case $ACTION_SEND_CHAT:
		echo send_chat($request);
	case $ACTION_GET_CHAT:
		echo get_chat($request);
	case $ACTION_GET_GAMESTATE:
		echo get_gamestate($request);
	case $ACTION_PLAYER_EVENT:
		echo player_event($request);
}

?>
