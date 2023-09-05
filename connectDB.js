
var dbUrl = "jdbc:google:mysql://****"
var dbUser = "mondobattle";
var dbPassword = "mondobattle";
function readFromTable() {
  try {
    const conn = Jdbc.getCloudSqlConnection(dbUrl, dbUser, dbPassword);
    const start = new Date();
    const stmt = conn.createStatement();
    stmt.setMaxRows(1000);
    const results = stmt.executeQuery('SELECT * FROM player');
    const numCols = results.getMetaData().getColumnCount();

    while (results.next()) {
      let rowString = '';
      for (let col = 0; col < numCols; col++) {
        rowString += results.getString(col + 1) + '\t';
      }
      console.log(rowString);
    }

    results.close();
    stmt.close();

    const end = new Date();
    console.log('Time elapsed: %sms', end - start);
  } catch (err) {
    // TODO(developer) - Handle exception from the API
    console.log('Failed with an error %s', err.message);
  }
}

function doGet(e) {
  readFromTable();
  // 取得輸入參數
  var params = e.parameter;
  let account = params.account;
  let password = params.password;
  let mode = params.mode;

  var json;
  if(mode=="create"){
    if(createNewAccount(account, password))
      json = {"ResultCode": 1};
    else
      json = { "ResultCode": 2, "Message": "Account already existed." };

  }else if(mode=="login"){
    if (checkPermissions(account, password))
      json = { "ResultCode": 1 };
    else
      json = { "ResultCode": 2, "Message": "Wrong account or password." };
  }

  var dataExportFormat = JSON.stringify(json);
  return ContentService.createTextOutput(dataExportFormat).setMimeType(ContentService.MimeType.JSON);
}
function createTable() {
  try {
    const conn = Jdbc.getCloudSqlConnection(dbUrl,dbUser,dbPassword);
    conn.createStatement().execute('CREATE TABLE player ' +
      '(account VARCHAR(255), password VARCHAR(255), ' +
      'ID INT NOT NULL AUTO_INCREMENT, PRIMARY KEY(ID));');
  } catch (err) {
    // TODO(developer) - Handle exception from the API
    console.log('Failed with an error %s', err.message);
  }
}
function createNewAccount(account, password){
  var id = checkIfExisted(account);
  if(id<0)//account already existed.
    return false;
  
  if(writeOneRecord(id, account, password))
    return true;
  else  //error
    return false;
}

function writeOneRecord(id, account, password) {
  try {
    const conn = Jdbc.getCloudSqlConnection(dbUrl,dbUser,dbPassword);
    const stmt = conn.prepareStatement('INSERT INTO player ' +
      '(ID, account, password) values (?, ?, ?)');
    stmt.setString(1, id);
    stmt.setString(2, account);
    stmt.setString(3, password);
    stmt.execute();
  } catch (err) {
    console.log('Failed with an error %s', err.message);
    return false;
  }
  return true;
}

function checkIfExisted(account){
  try {
    const conn = Jdbc.getCloudSqlConnection(dbUrl,dbUser,dbPassword);
    const stmt = conn.createStatement();
    stmt.setMaxRows(1000);
    const results = stmt.executeQuery('SELECT account FROM player');

    while (results.next()) {
      let rowString = results.getString(1);
      
      if(rowString == account){
            results.close();
            stmt.close();
            return -1;
      }
    }
    var id = results.getRow();
    results.close();
    stmt.close();
    return id;
  } catch (err) {
    console.log('Failed with an error %s', err.message);
    return -1;
  }
}
function checkPermissions(account, password) {
  try {
    const conn = Jdbc.getCloudSqlConnection(dbUrl,dbUser,dbPassword);
    const stmt = conn.createStatement();
    stmt.setMaxRows(1000);
    const results = stmt.executeQuery('SELECT * FROM player WHERE account="'+account+'"');

    while (results.next()) {
      let target_password = results.getString(2);
      if (target_password==password){
        return true;
      }
    }
    results.close();
    stmt.close();
  } catch (err) {
    console.log('Failed with an error %s', err.message);
    return false;
  }
  return false;
}