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
		var log = {"path": pathname, "action": "HB_PLAYERDATA", "msg": JSON.stringify(playerData)};
		console.log("SAVING GAME DATA TO DB");
		console.log("LOG: ");
		console.log(log);
		$.ajax({
			type: "POST",
			url: "/hello/log/",
			data: log,
			success: function(response) {
				console.log("success!");
				var currGame = {{ currentGame }};
				if(currGame == "game1") {
					document.location.href = '/imiSurveyGame1';
				} else {
					document.location.href = '/imiSurveyGame2';
				}
			},
			error: function(xhr, status, error) {
				alert(xhr.responseText);
			},
		});
	
  },

});