$('#btns a').click(function (e) {
   var This = this;
   if($('#btns').hasClass("disabled")){
      e.preventDefault();
   } else {
      var sid = $(this).attr("data-sid");
      if (sid == '') {
         $('#myModal').modal('show');
      } else {
         if ($(this).attr("id") == "btn_checkout") {
            $("form[name='gform']").attr("action", "/mall/cart/Settle");
            $("form[name='gform']").submit();
         } else if ($(this).attr("id") == "btn_caret") {
            $("form[name='gform']").attr("action", "/mall/cart/create");
            $("form[name='gform']").submit();
         }
      }
   }
});

function checkLogin(sid) {
   if (sid == '') {
      $('#myModal').modal('show');
   } else {
      $('form')[0].submit();
   }
}
$('#myModal').find('input').change(function () {
   if ($('#user').val() == '' || $('#pwd').val() == '') {
      $('#acount_submit').attr("disabled", true);
   } else {
      $('#acount_submit').attr("disabled", false);
   }
});
$('#acount_submit').click(function () {
   $.post('/mall/account/signinByAjax', { user: $('#user').val(), pwd: $('#pwd').val() }, function (res) {
      if (res == true) {
         window.location.reload();
      }
   });
});