import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-show-department',
  templateUrl: './show-department.component.html',
  styleUrls: ['./show-department.component.css']
})
export class ShowDepartmentComponent implements OnInit {

  constructor(private service: SharedService) { }

  DepartmentList: any = [];

  ModalTitle: string = "";
  ActivateAddEditDepartment: boolean = false;
  dep: any;
  wasAdded:boolean=false;

  ngOnInit(): void {
    this.refreshDepartmentList();
  }

  addClick() {
    this.dep = {
      DepartmentId: 0,
      DepartmentName: ""
    };

    this.ModalTitle = "Add Department";
    this.ActivateAddEditDepartment = true;
    this.wasAdded=false;
  }

  editClick(item: any) {
    this.dep = item;
    this.ModalTitle = "Edit Department";
    this.ActivateAddEditDepartment = true;
    this.wasAdded=false;
  }

  deleteClick(item: any) {
    if (confirm("Are you sure?")) {
      this.service.deleteDepartment(item.DepartmentId).subscribe(data => {
        this.refreshDepartmentList();
        alert(data.toString());
      });
    }
  }

  closeClick() {

    this.ActivateAddEditDepartment = false;
    this.refreshDepartmentList();
    this.wasAdded=true;
  }

  refreshDepartmentList() {
    this.service.getDepartmentList().subscribe(data => {
      this.DepartmentList = data;
      console.log("eu " + data[0].DepartmentId + ' ' + data + ' ');
    });
  }

}
