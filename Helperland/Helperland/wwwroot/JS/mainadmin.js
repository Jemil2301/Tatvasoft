$(document).ready(function() {
	
		$("#table-id").DataTable({
			"dom":"tlp",
			'dom': "<'row'<'col-sm-12'tr>>" +
         	"<'row'<'col-sm-3'l>,<'col-sm-9'p>>",
         	"lengthMenu": [[5,10, 25, 50, -1], [5,10, 25, 50, "All"]],
            'language': {
	 					'oPaginate': {
	   								 'sNext': '<i class="fa fa fa-caret-right"></i>',
	   							     'sPrevious': '<i class="fa fa fa-caret-left"></i>',
	   
	  							     }
	   					},
	   		'aoColumnDefs': [{
        						'bSortable': false,
        						'aTargets': [-1] /* 1st one, start by the right */
    						}],
    		"drawCallback": function( settings ) {
       						 $("#table-id").wrap( "<div class='table-responsive'></div>" );
			},
			buttons: [{
				extend: 'csv',
				exportOptions: {
					columns: 'th:not(:last-child)'
				}
			}]
			



		});
	$('#button_export_excel').click(() => {

		$('#table-id').DataTable().buttons(0, 0).trigger()
	});
		 var oTable = $('#table-id').DataTable();

    $('#searchButton').click(function () {
      	if($("#usrtype").val() ==null)
      	{
      	oTable.column([0]).search($("#usrname").val()).draw();
        oTable.column([1]).search($("#phn").val()).draw();
        oTable.column([4]).search($("#zipcode").val()).draw();
    	}
      	else
      	{
     	oTable.column([0]).search($("#usrname").val()).draw();
        oTable.column([3]).search($("#usrtype").val()).draw();
        oTable.column([1]).search($("#phn").val()).draw();
        oTable.column([4]).search($("#zipcode").val()).draw();
    	}
    });
	$('#clearButton').click(function () {
		$("#usrname").val("");
		$("#usrtype").val("");
		$("#phn").val("");
		$("#zipcode").val("");
		
	     	oTable.column([0]).search($("#usrname").val()).draw();
	        oTable.column([3]).search('').draw();
	        oTable.column([1]).search($("#phn").val()).draw();
	        oTable.column([4]).search($("#zipcode").val()).draw();
	   

	});

});

$(document).ready(function(){

	$("#tableservicerequest").DataTable({

		"dom":"tlp",
			'dom': "<'row'<'col-sm-12'tr>>" +
         	"<'row'<'col-sm-3'l>,<'col-sm-9'p>>",
         	"lengthMenu": [[5,10, 25, 50, -1], [5,10, 25, 50, "All"]],
            'language': {
	 					'oPaginate': {
	   								 'sNext': '<i class="fa fa fa-caret-right"></i>',
	   							     'sPrevious': '<i class="fa fa fa-caret-left"></i>',
	   
	  							     }
	   					},
	   		'aoColumnDefs': [{
        						'bSortable': false,
        						'aTargets': [-1] /* 1st one, start by the right */
    						}],
	   		"drawCallback": function( settings ) {
       						 $("#tableservicerequest").wrap( "<div class='table-responsive'></div>" );
   							 },
   							 "searching": true

	});

	
	
	var minDate, maxDate;
	// Custom filtering function which will search data in column four between two values
	$.fn.dataTable.ext.search.push(
		function (settings, data, dataIndex) {
			var min = minDate.val();
			var max = maxDate.val();
			var parts = (data[1].slice(0, 10)).split("-");
			var date = new Date(parseInt(parts[2], 10),
				parseInt(parts[1], 10) - 1,
				parseInt(parts[0], 10));



			if (
				(min === null && max === null) ||
				(min === null && date <= max) ||
				(min <= date && max === null) ||
				(min <= date && date <= max)
			) {


				return true;
			}
			return false;
		}
	);


	// DataTables initialisation


	minDate = new DateTime($('#fromdate'), {
		format: 'DD-MM-YYYY'
	});
	maxDate = new DateTime($('#todate'), {
		format: 'DD-MM-YYYY'
	});
	var table = $("#tableservicerequest").DataTable();
	table.on('draw', function () {
		$('div[onload]').trigger('onload');
	});
	$('#searchbtn').click(function () {
		table.column([0]).search($("#serviceid").val()).draw();
		table.column([2]).search($("#usrname").val()).draw();
		table.column([3]).search($("#Spname").val()).draw();
		table.column([5]).search($("#status").val()).draw();
		table.draw();

	});
	$('#clearbtn').click(function () {
		location.reload();
		$("#serviceid").val("");
		$("#usrname").val("");
		$("#Spname").val("");
		$("#status").val("");
		table.column([0]).search($("#serviceid").val()).draw();
		table.column([2]).search($("#usrname").val()).draw();
		table.column([3]).search($("#Spname").val()).draw();
		table.column([5]).search($("#status").val()).draw();


	});

});

