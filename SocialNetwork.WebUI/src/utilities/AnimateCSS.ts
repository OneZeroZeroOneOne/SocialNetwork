export default function animateCSS(element: Element, animation: string, callback: Function|undefined = undefined, prefix: string = 'animate__', suffix: string = "animated")
{
  // We create a Promise and return it
  return new Promise((resolve, reject) => {
    const animationName = `${prefix}${animation}`;

    element.classList.add(`${prefix}${suffix}`, animationName);

    // When the animation ends, we clean the classes and resolve the Promise
    function handleAnimationEnd() {
        element.classList.remove(`${prefix}${suffix}`, animationName);
        element.removeEventListener('animationend', handleAnimationEnd);
        
        if (callback !== undefined)
            callback()

        resolve('Animation ended');
    }

    element.addEventListener('animationend', handleAnimationEnd);
  });
}
