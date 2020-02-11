<?php
/**
 * Created by PhpStorm.
 * User: filha
 * Date: 22.09.2019
 * Time: 2:15
 */

require_once __DIR__.'/../config/DB.php';

class DatabaseConnector
{
    /**
     * @var DatabaseConnector
     */
    private static $connector;

    private $dbConnection = null;

    private function __construct()
    {
        try {
            $this->dbConnection = new PDO(
                'mysql:host='.DB::Host.';dbname='.DB::Db,
                DB::User,
                DB::Password
            );
            $this->dbConnection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch (PDOException $e) {
            exit($e->getMessage());
        }
    }

    /**
     * @return DatabaseConnector
     */
    public static function create(){
        if(!isset(DatabaseConnector::$connector)){
            DatabaseConnector::$connector = new DatabaseConnector();
        }
        return DatabaseConnector::$connector;
    }

    public function getConnection()
    {
        return $this->dbConnection;
    }
}