mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  HelloString: function (str) {
    window.alert(Pointer_stringify(str));
  },

  PrintFloatArray: function (array, size) {
    for(var i = 0; i < size; i++)
    console.log(HEAPF32[(array >> 2) + i]);
  },

  AddNumbers: function (x, y) {
    return x + y;
  },

  StringReturnValueFunction: function () {
    var returnStr = "bla";
    var bufferSize = lengthBytesUTF8(returnStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(returnStr, buffer, bufferSize);
    return buffer;
  },

  BindWebGLTexture: function (texture) {
    GLctx.bindTexture(GLctx.TEXTURE_2D, GL.textures[texture]);
  },
  
  sendToDB: function (playerData) {
		var pathname = window.location.pathname;
		var log = {"path": pathname, "action": "HB_PLAYERDATA", "msg": Pointer_stringify(playerData)};
		console.log("SAVING GAME DATA TO DB");
		console.log("LOG: ");
		console.log(log);
		$.ajax({
			type: "POST",
			url: "/hello/log/",
			data: log,
			success: function(response) {
				console.log("success!");
	
			},
			error: function(xhr, status, error) {
				alert(xhr.responseText);
			},
		});
	
  },
  
  
  saveToDB: function (saveData) {
		var pathname = window.location.pathname;
		//alert("saveData = " + Pointer_stringify(saveData) + "endOfLine");
		var log = {"path": pathname, "action": "SAVE", "msg": Pointer_stringify(saveData)};
		console.log("SAVING GAME STATE TO DB");
		console.log("LOG: ");
		console.log(log);
		$.ajax({
			type: "POST",
			url: "/hello/log/",
			data: log,
			success: function(response) {
				console.log("success!");
	
			},
			error: function(xhr, status, error) {
				alert(xhr.responseText);
			},
		});
	
  },  
  
  // textContent should be a string already. So loadData should be a string too.
  loadFromDB: function () {
		var loadDataHTML = document.getElementById('loadGameID');
		var loadDataRaw = loadDataHTML.textContent;
		//alert("Type of loadDataRaw = " + typeof loadDataRaw)
		// I don't think JSON stringify is necessary here, but just in case
		var loadData = JSON.stringify(loadDataHTML.textContent)
		//alert("Type of loadData = " + typeof loadData)
		
		// this is showing actual quotes around the JSON brackets - maybe these need to go
		//alert("loadData = " + loadData + "endOfLine")
		
		// this is necessary to return a string in a format C# understands
		var bufferSize = lengthBytesUTF8(loadData) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(loadData, buffer, bufferSize);
		//alert("Type of loadDataToUTF8 = " + typeof buffer)

		//alert("loadDataToUTF8 = " + buffer + "endOfLine")

		return buffer;
		//return loadData;
  },   
   

});