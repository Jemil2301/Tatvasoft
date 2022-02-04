
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












