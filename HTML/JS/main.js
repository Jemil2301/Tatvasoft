let nav =document.querySelector(".navigation-wrap")

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


