//$(document).ready(function () {
//    $('#carouselControls').carousel();
//});

//$(".carousel-control-prev").click(function () {
//    console.log("Previous Butoon click")
//});
//$(".carousel-control-next").click(function () {
//    console.log("Next Button Click")
//});


document.addEventListener("DOMContentLoaded", function () {
    
    const updateBackgroundImage = () => {
        const activeItem = document.querySelector(".carousel-item.active .carousel-background");
        if (activeItem) {
            const backgroundImage = window.getComputedStyle(activeItem).backgroundImage;
            const url = backgroundImage.match(/url\(["']?(.*?)["']?\)/)?.[1];
            if (url) {
                document.body.style.backgroundImage = `url(${url})`;
            }
        }
    };

  
    const preloadImages = () => {
        const carouselItems = document.querySelectorAll(".carousel-item .carousel-background");
        carouselItems.forEach(item => {
            const backgroundImage = window.getComputedStyle(item).backgroundImage;
            const url = backgroundImage.match(/url\(["']?(.*?)["']?\)/)?.[1];
            if (url) {
                const img = new Image();
                img.src = url;
            }
        });
    };

    preloadImages();
    updateBackgroundImage();

   
    const carouselElement = document.querySelector('#carouselControls');
    if (carouselElement) {
        carouselElement.addEventListener("slid.bs.carousel", function (event) {
            updateBackgroundImage();
        });
    }

    console.log("Carousel functionality initialized");
});

