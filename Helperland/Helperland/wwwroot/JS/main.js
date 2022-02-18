
let nav = document.querySelector(".navigation-wrap")

window.onscroll = function () {
  if(window.matchMedia('(min-width: 993px)').matches && document.documentElement.scrollTop >20){
    nav.classList.add("scroll-on");
    var yourImg = document.getElementById('logo');
    if(yourImg && yourImg.style) {
        yourImg.style.height = '54px';
        yourImg.style.width = '73px';

      }

  }
  else{
    nav.classList.remove("scroll-on");
    var yourImg = document.getElementById('logo');
    if(yourImg && yourImg.style) {
        yourImg.style.height = '130px';
        yourImg.style.width = '175px';

      }
  }
}



$(document).ready(function() {
  console.log('welcome to jquery')
  $('.close').on('click', function(){
      // console closest element with class=".product-container"
      console.log($(this).closest('.Privacypolicy'))
      // hide this selected element
      $(this).closest('.Privacypolicy').hide()
  });


  $(".cancle").click(function () {
    $(this).text('cancled').css("background-color", "#c24c4c")
    
  }); 

  $(".first-2").click(function(){
    $(".first-2 .num").css("display","none")
  });
});


const details = document.querySelectorAll("details");

// Add the onclick listeners.
details.forEach((targetDetail) => {
  targetDetail.addEventListener("click", () => {
    // Close all the details that are not targetDetail.
    details.forEach((detail) => {
      if (detail !== targetDetail) {
        detail.removeAttribute("open");
      }
    });
  });
});


$(document).ready(function() {
  var countrow=$(".datatable tbody tr").length;
  $("#numbertr").text("Total Record:"+countrow);
  
  $(".rateYo").rateYo({
    rating: 4,
    starWidth: "17px",
    spacing: "2px",
    ratedFill:"#ecb91c",
    fullStar: true,
    precision: 2,
    normalFill:"#d4d4d4",



});
   $(".rateYo").rateYo().on("rateyo.change", function (e, data) {
    var rating = data.rating;
    $(this).parent().find('.ratingnum').text(rating);

   });


});


$(document).ready(function() {
    $('#table-id').DataTable({
        "bFilter": false,
        "lengthMenu": [[5,10, 25, 50, -1], [5,10, 25, 50, "All"]],
        "dom":"tlip",
         'dom': "<'row'<'col-sm-12'tr>>" +
         "<'row'<'col-sm-3'l>,<'col-sm-4'i>,<'col-sm-4'p>>",
          "pagingType": "full_numbers",
          "oLanguage": {
"sInfo": "Total Records: _TOTAL_"
}, 'language': {
 'oPaginate': {
   'sNext': '<i class="fa fa-chevron-right"></i>',
   'sPrevious': '<i class="fa fa-chevron-left"></i>',
   'sFirst': '<i class="fa fa-step-backward"></i>',
   'sLast': '<i class="fa fa-step-forward"></i>',

   }
   } ,
 
   "drawCallback": function( settings ) {
        $("#table-id").wrap( "<div class='table-responsive'></div>" );
    },
    'aoColumnDefs': [{
        'bSortable': false,
        'aTargets': [-1] /* 1st one, start by the right */
    }]

     

    });
  
   


} );


$(document).ready(function () {
    if ($('#invoiceadd').is(":checked"))
        $(".invoice-address").show();


});

function valueChanged() {
    if ($('#invoiceadd').is(":checked"))
        $(".invoice-address").show();
    else
        $(".invoice-address").hide();
}

function showadd() {
    document.getElementById("addaddress2").style.display = "block";
    document.getElementById("addaddress1").style.display = "none";
}

function showbtn() {
    document.getElementById("addaddress2").style.display = "none";
    document.getElementById("addaddress1").style.display = "block";
}

function change1() {
    var checkBox = document.getElementById("cabinets");
    var text = document.getElementById("img1");
    if (checkBox.checked == true) {
        text.style.border = "3px solid #B0C9CE";
    }
}

$(document).ready(function () {
    $('#cabinets').change(function () {
        if ($(this).is(":checked")) {
            $('#img1').addClass("clicked");
            $('#img1').removeClass("unclicked");
            $('#imgun11').hide();
            $('#imgun12').show();
            $('.ex11').show();
            $('.ex21').show();
            $('#totatime option:selected').next().prop('selected', true);
            Extrahours = Extrahours + 0.5;
            $("#extraHrs").val(Extrahours);
            $("#one").val(true);
        } else {
            $('#img1').addClass("unclicked");
            $('#img1').removeClass("clicked");
            $('#imgun11').show();
            $('#imgun12').hide();
            $('.ex11').hide();
            $('.ex21').hide();
            $('#totatime option:selected').prev().prop('selected', true);
            Extrahours = Extrahours - 0.5;
            $("#extraHrs").val(Extrahours);
            $("#one").val(false);
        }

        var x = $("#basictime").text();
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);


    });

    $('#fridge').change(function () {
        if ($(this).is(":checked")) {
            $('#img2').addClass("clicked");
            $('#img2').removeClass("unclicked");
            $('#imgun21').hide();
            $('#imgun22').show();
            $('.ex12').show();
            $('.ex22').show();
            $('#totatime option:selected').next().prop('selected', true);
            Extrahours = Extrahours + 0.5;
            $("#extraHrs").val(Extrahours);
            $("#two").val(true);
           
        } else {
            $('#img2').addClass("unclicked");
            $('#img2').removeClass("clicked");
            $('#imgun21').show();
            $('#imgun22').hide();
            $('.ex12').hide();
            $('.ex22').hide();
            $('#totatime option:selected').prev().prop('selected', true);
            Extrahours = Extrahours - 0.5;
            $("#extraHrs").val(Extrahours);
            $("#two").val(false);
           
        }


        var x = $("#basictime").text();
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);
    });

    $('#oven').change(function () {
        if ($(this).is(":checked")) {
            $('#img3').addClass("clicked");
            $('#img3').removeClass("unclicked");
            $('#imgun31').hide();
            $('#imgun32').show();
            $('.ex13').show();
            $('.ex23').show();
            $('#totatime option:selected').next().prop('selected', true);
            Extrahours = Extrahours + 0.5;
            $("#extraHrs").val(Extrahours);
            $("#three").val(true);

        } else {
            $('#img3').addClass("unclicked");
            $('#img3').removeClass("clicked");
            $('#imgun31').show();
            $('#imgun32').hide();
            $('.ex13').hide();
            $('.ex23').hide();
            $('#totatime option:selected').prev().prop('selected', true);
            Extrahours = Extrahours - 0.5;
            $("#extraHrs").val(Extrahours);
            $("#three").val(false);
        }


        var x = $("#basictime").text();
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);
    });

    $('#laundry').change(function () {
        if ($(this).is(":checked")) {
            $('#img4').addClass("clicked");
            $('#img4').removeClass("unclicked");
            $('#imgun41').hide();
            $('#imgun42').show();
            $('.ex14').show();
            $('.ex24').show();
            $('#totatime option:selected').next().prop('selected', true);
            Extrahours = Extrahours + 0.5;
            $("#extraHrs").val(Extrahours);
            $("#four").val(true);
        } else {
            $('#img4').addClass("unclicked");
            $('#img4').removeClass("clicked");
            $('#imgun41').show();
            $('#imgun42').hide();
            $('.ex14').hide();
            $('.ex24').hide();
            $('#totatime option:selected').prev().prop('selected', true);
            Extrahours = Extrahours - 0.5;
            $("#extraHrs").val(Extrahours);
            $("#four").val(false);
        }


        var x = $("#basictime").text();
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);
    });

    $('#windows').change(function () {
        if ($(this).is(":checked")) {
            $('#img5').addClass("clicked");
            $('#img5').removeClass("unclicked");
            $('#imgun51').hide();
            $('#imgun52').show();
            $('.ex15').show();
            $('.ex25').show();
            $('#totatime option:selected').next().prop('selected', true);
            Extrahours = Extrahours + 0.5;
            $("#extraHrs").val(Extrahours);
            $("#five").val(true);
        } else {
            $('#img5').addClass("unclicked");
            $('#img5').removeClass("clicked");
            $('#imgun51').show();
            $('#imgun52').hide();
            $('.ex15').hide();
            $('.ex25').hide();
            $('#totatime option:selected').prev().prop('selected', true);
            Extrahours = Extrahours - 0.5;
            $("#extraHrs").val(Extrahours);
            $("#five").val(false);
        }

        var x = $("#basictime").text();
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);
    });









});



/*$(document).ready(function() {

$('#checkava').click(function(e) {
  e.preventDefault();
  var link = $('#mytabs .active').next().children('a').attr('href');

  $('#pills-tab button[href="' + link + '"]').tab('show');
});
});*/


var totaltime;
var totalprice;
var Effectiveprice;
var Extrahours = 0;
$(document).ready(function () {
    $("#txtServiceStartDate").change(function () {
        var x = $("#txtServiceStartDate").val();
        $(".dateinsummary").text(x);
    });
    $("#timeselect").change(function () {
        var x = $("#timeselect").val();
        $(".timeinsummary").text(x);
    });


    $("#totatime").change(function () {
        var x = $("#totatime").val();
        $(".basictime").text(x);
        totaltime = parseFloat(x);
        if ($('#cabinets').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#fridge').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#oven').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#laundry').is(":checked")) {
            totaltime = totaltime + 0.5;
        }
        if ($('#windows').is(":checked")) {
            totaltime = totaltime + 0.5;
        }

        $(".sumoftime").text(totaltime);
        totalprice = totaltime * 18;
        $(".totalprice").text(totalprice);
        $(".totalprice1").text(totalprice);
        Effectiveprice = (totalprice * 80) / 100;
        $(".Effectiveprice").text(Effectiveprice);

    });





    $('.sumoftime').on('DOMSubtreeModified', function () {
        var x = $("#sumoftime").text();
        $("#serviceHrs").val(x);
    });
    $('.totalprice').on('DOMSubtreeModified', function () {
        var x = $("#totalprice").text();
        $("#subtotal").val(x);
    });
    $('.totalprice1').on('DOMSubtreeModified', function () {
        var x = $("#totalprice1").text();
        $("#totalcost").val(x);
    });

   
      $("#txtServiceStartDate").datepicker({ dateFormat: 'dd-mm-yy', minDate: 0 }).val();
    $("#txtServiceStartDate").datepicker("setDate", "0");



    $("#streatname").prop('required', true);
    $("#housenumber").prop('required', true);


    $("#txtServiceStartDate").change(function () {
        var x = $("#txtServiceStartDate").val();
        $("#servicedate").val(x);
    });
    $("#timeselect").change(function () {
        var x = $("#timeselect").val();
        $("#servicetime").val(x);
    });
    $("#commentstxt").change(function () {
        var x = $("#commentstxt").val();
        $("#sevicecomments").val(x);
    });

    $("#haspets").change(function () {
        if ($(this).is(":checked"))
        {
            $("#servicehaspats").val(true);
        }
        else
        {
            $("#servicehaspats").val(false);

        }
        
        
    });



});
var tomorrow = new Date();
var datefirst = tomorrow.getDate() + "-" + (tomorrow.getMonth()+1)+ "-" + tomorrow.getFullYear();
document.getElementById("dateinsummary").innerHTML = datefirst;
document.getElementById("dateinsummary1").innerHTML = datefirst;
document.getElementById("servicedate").value = datefirst;

function addaddress() {
    $(document).ready(function () {
        var streat = $("#streatname").val();
        var house = $("#housenumber").val();
        var postalcode = $("#postalcode").val();
        var city = $("#city").val();
        var phone = $("#phone").val();
        var id = Math.floor(Math.random() * 10000);
        if (streat != "" && house != "") {
            if (phone == "") {
                $("#addressul").append("<li class='ng - valid ng - dirty ng - touched'><label><input formcontrolname='address' name='address' type='radio' id='" + id + "' class='ng-valid ng-dirty ng-touched'/><span class='address-block'><b>Address:</b>" + streat + "," + house + "</span><span><b>Phone number: Null</b></span><span class='radio-pointer'></span></label></li>");
                $('#addaddress2').hide();
                $('#addaddress1').show();

            }
            else {
                $("#addressul").append("<li class='ng - valid ng - dirty ng - touched'><label><input formcontrolname='address' name='address' type='radio' id='" + id + "' class='ng-valid ng-dirty ng-touched'/><span class='address-block'><b>Address:</b>" + streat + "," + house + "</span><span><b>Phone number:" + phone + "</b></span><span class='radio-pointer'></span></label></li>");
                $('#addaddress2').hide();
                $('#addaddress1').show();
            }
        }

        $("#ad1").val(streat);
        $("#ad2").val(house);
        $("#ct").val(city);
        $("#psc").val(postalcode);
        $("#mo").val(phone);

    });


}





