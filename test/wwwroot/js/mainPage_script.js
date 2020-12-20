$(document).ready(function (){
    window.currentPage = 1;

    $('#movies-today-list').paginateList(4, function (pageLink, pageNum, dataTarget){
        pageLink.click(function (){
            if(window.currentPage != pageNum)
            {
                $(dataTarget).collapse('toggle');
                window.currentPage = pageNum;
            }
        });
    });

    $('#top-movies-slick').slick({
        centerMode: true,
        centerPadding: '1.5%',
        slidesToShow: 5,
        swipeToSlide: true,
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '100px',
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 996,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 3
                }
            },
            {
                breakpoint: 820,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '150px',
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 576,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '0px',
                    slidesToShow: 1
                }
            }
        ]
    });

    $('#top-movies-slick').removeClass("d-none");
});

(function( $ ){
    $.fn.paginateList = function(number_of_elements, pageLinkCallback) {
        let parent = this.parent();
        let this_id = $(this).attr('id');
        let $this = $(this);
        let paginate_nav = $(document.createElement('nav'));
        paginate_nav.addClass( `${this_id}-page-nav`)
        let paginate_list = $(document.createElement('ul'));
        paginate_list.addClass('pagination');
        let accordion = $(document.createElement("div"));
        accordion.attr('id',`${this_id}-accordion`);

        let current_list = this;
        Array.from(this.children()).forEach(function (list_item, index){
            let page_num = Math.floor(index/number_of_elements)+1;
            if(index%number_of_elements === 0){
                current_list = $(document.createElement("div"))
                accordion.append(current_list);
                current_list.attr('class', $this.attr('class'));
                let new_id = `${this_id}-group${page_num}`;
                current_list.attr("id",new_id);
                current_list.addClass("collapse");
                if(page_num === 1) current_list.addClass('show');
                current_list.attr('data-parent', `#${accordion.attr('id')}`);
                let pageLink = $(`<button class="page-link">${page_num}</button>`).addClass('page-link-dark-transparent');
                let pageItem = $(document.createElement('li')).addClass('page-item').append(pageLink);
                paginate_list.append(pageItem);
                pageLinkCallback(pageLink, page_num, `#${new_id}`);
            }

            $(list_item).appendTo(current_list);
        });
        paginate_nav.append(paginate_list);
        parent.append(paginate_nav.clone(true));
        parent.append(accordion);
        parent.append(paginate_nav);
        let event = document.createEvent('Event');
        event.initEvent('paginateCompleted', true, true);
        this[0].dispatchEvent(event);
        this.remove();
    };
})( jQuery );