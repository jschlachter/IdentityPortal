var domready = function(){
  // Handler when the DOM is fully loaded
};

if (
    document.readyState === "complete" ||
    (document.readyState !== "loading" && !document.documentElement.doScroll)
) {
  domready();
} else {
  document.addEventListener("DOMContentLoaded", domready);
}