$(document).on('click', '#close-preview', function () {
  $('.image-preview').popover('hide');
  // Hover befor close the preview
  $('.image-preview').hover(
    function () {
      $('.image-preview').popover('show');
    },
    function () {
      $('.image-preview').popover('hide');
    }
  );
});
$('#btnNew').click(() => {
  $('#txtName').val('');
  $('#txtDescricao').val('');
  $('#inpImage').val('');
  $('#txtIdent').val('');
});
$('#btnSubmit').click(() => location.reload(true));
$(document).ready(function () {
  $(function () {
    // Create the close button
    var closebtn = $('<button/>', {
      type: "button",
      text: 'x',
      id: 'close-preview',
      style: 'font-size: initial;',
    });
    closebtn.attr("class", "close pull-right");
    // Set the popover default content
    $('.image-preview').popover({
      trigger: 'manual',
      html: true,
      title: "<strong>Preview</strong>" + $(closebtn)[0].outerHTML,
      content: "Não existe imagem inserida",
      placement: 'bottom'
    });
    // Clear event
    $('.image-preview-clear').click(function () {
      $('.image-preview').attr("data-content", "").popover('hide');
      $('.image-preview-filename').val("");
      $('.image-preview-clear').hide();
      $('.image-preview-input input:file').val("");
      $(".image-preview-input-title").text("Browse");
    });
    // Create the preview image
    $(".image-preview-input input:file").change(function () {
      var img = $('<img/>', {
        id: 'dynamic',
        width: 250,
        height: 200
      });
      var file = this.files[0];
      var reader = new FileReader();
      // Set preview image into the popover data-content
      reader.onload = function (e) {
        $(".image-preview-input-title").text("Alterar");
        $(".image-preview-clear").show();
        $(".image-preview-filename").val(file.name);
        img.attr('src', e.target.result);
        $('#content').val(e.target.result);
        $(".image-preview").attr("data-content", $(img)[0].outerHTML).popover("show");
      }
      reader.readAsDataURL(file);
    });
  });
});

function showOptions(element) {
  $(element).children('.card-body').children('.options').css("opacity", 1);
}

function releaseOptions(element) {
  $(element).children('.card-body').children('.options').css("opacity", 0);
}

function onEnter(element) {
  if ((element.value !== undefined && element.value.length >= 0) || $(this).attr('placeholder') !== null) {
    element.parentNode.querySelector("label").classList.add("active");
  }
}

function onExit(element) {
  if ((element.value !== undefined && element.value.length == 0) || $(this).attr('placeholder') === null) {
    element.parentNode.querySelector("label").classList.remove("active");
  }
}

function deleteSuper(element) {
  let c = $(element).parents('.col-lg-3');
  let id = c.attr('id');
  $.ajax({
    url: `Api/deletehero/${id}`,
    type: 'DELETE',
    success: (r) => {
      c.remove();
      location.reload();
    }
  });
}

function editSuper(element) {
  let id = $(element).parents('.col-lg-3').attr('id');
  $.ajax({
    url: `Api/getbyid/${id}`,
    type: 'GET',
    success: (r) => {
      $('#txtIdent').val(r.id);
      $('#txtName').val(r.name);
      $('#txtDescricao').val(r.description);
      $(".image-preview-clear").show();
      $(".image-preview-filename").val(r.name + ".jpg");
      $('#content').val(r.image);

      setTimeout(() => {
        $('#txtName + label').css('top', '-60%');
        $('#txtDescricao + label').css('top', '-11%');
      }, 300);
      clearTimeout();
    }
  });
}

function setFavorite(element) {
  let c = $(element).parents('.col-lg-3');
  let father = $(element).parent('.favorite');
  let id = c.attr('id');
  $.ajax({
    url: `Api/setfavorite/${id}`,
    type: 'GET',
    success: (r) => {
      if (father.css("background-color") == "rgb(0, 0, 0)")
        father.css('background-color', "#920000");
      else
        father.css('background-color', "#000000");
    }
  });
}