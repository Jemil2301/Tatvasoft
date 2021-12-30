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
        						'aTargets': [-1,-4,-6,-7] /* 1st one, start by the right */
    						}],
    		"drawCallback": function( settings ) {
       						 $("#table-id").wrap( "<div class='table-responsive'></div>" );
   							 },


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
	});

	$(function(){
    		$("#todate").datepicker();
    		$("#fromdate").datepicker();
	});
	var table=$("#tableservicerequest").DataTable();
	$('#serviceid').on( 'keyup', function () {
    table.search(this.value).draw();
		});

});

