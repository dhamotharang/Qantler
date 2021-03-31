import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {NgxSpinnerService} from 'ngx-spinner';
import {ConfirmationService, MessageService} from 'primeng/api';
import {environment} from '../../../environments/environment';
import {MdmServiceService} from '../../services/mdm/mdm-service.service';
import {ProfileService} from '../../services/profile.service';
import {QrcService} from '../../services/qrc.service';

@Component({
  selector: 'app-qrc',
  templateUrl: './qrc.component.html',
  styleUrls: ['./qrc.component.css']
})
export class QrcComponent implements OnInit {
  qrcs: any;
  categories: any;
  Allcategories: any;
  qrc: number;
  category: number;
  pageSize: number;
  pageNumber: number;
  totalUsersCount: number;
  users: any;
  showAddQRCForm: boolean;
  showAddUsersForm: boolean;
  QRCForm: FormGroup;
  user: any;
  newUsers: any[];
  selectedNewUsers: any[];
  newUsersPageNo: number;
  newUsersPageSize: number;
  newUsersTotal: number;
  newUsersFirst: number;
  userProfile: boolean;
  publicUser: any;
  filterCategory: any;
  qrcFilterCategory: any;
  selectedUsers: any;
  userArray: any;
  subjectsInterested: string;

  constructor(
    private QRCService: QrcService, private profileService: ProfileService, private confirmationService: ConfirmationService,
    private MdMservice: MdmServiceService, private spinner: NgxSpinnerService, private messageService: MessageService) {
    this.QRCForm = new FormGroup({
      name: new FormControl(null, [Validators.required, Validators.maxLength(150)]),
      description: new FormControl(null, [Validators.required, Validators.maxLength(2000)]),
      createdBy: new FormControl(null),
      updatedBy: new FormControl(null),
      categoryIds: new FormControl(null, Validators.required),
      active: new FormControl(true)
    });
  }

  ngOnInit() {
    this.user = this.profileService.user;
    this.profileService.getUserDataUpdate().subscribe(() => {
      this.user = this.profileService.user;
    });
    this.publicUser = null;
    this.qrcs = [];
    this.userArray = [];
    this.selectedUsers = [];
    this.filterCategory = 0;
    this.qrcFilterCategory = 0;
    this.Allcategories = [];
    this.newUsersPageNo = 0;
    this.newUsersPageSize = 10000;
    this.newUsersTotal = 0;
    this.newUsersFirst = 0;
    this.categories = [];
    this.users = [];
    this.newUsers = [];
    this.selectedNewUsers = [];
    this.qrc = null;
    this.category = null;
    this.pageSize = 10;
    this.pageNumber = 1;
    this.totalUsersCount = 0;
    this.showAddQRCForm = false;
    this.userProfile = false;
    this.showAddUsersForm = false;
    this.getCategories();
  }

  getQrcList() {
    this.spinner.show();
    this.qrc = null;
    this.category = null;
    this.filterCategory = null;
    this.qrcs = [];
    this.users = [];
    this.totalUsersCount = 0;
    this.QRCService.getAllQRC(this.qrcFilterCategory).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.qrcs = res.returnedObject;
      } else {
        this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
      }
      this.spinner.hide();
    }, (error) => {
      this.messageService.add({severity: 'error', summary: 'Failed to retrieve QRC list', key: 'toast'});
      this.spinner.hide();
    });
  }

  getCategories() {
    this.MdMservice.getAllCategories(1).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.Allcategories = res.returnedObject;
      }
    });
  }

  getCategoryByQRC() {
    if (this.qrc) {
      this.users = [];
      this.spinner.show();
      this.QRCService.getCategoryByQRC(this.qrc).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.spinner.hide();
          this.category = res.returnedObject[0].id;
          this.filterCategory = res.returnedObject[0].id;
          this.getQRCUserList();
        } else {
          this.spinner.hide();
        }
      }, (error) => {
        this.spinner.hide();
      });
    }
  }

  getQRCUserList() {
    if (this.qrc && this.category) {
      this.spinner.show();
      this.QRCService.getQRCUsers(this.qrc, this.category, this.pageNumber, this.pageSize).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.users = res.returnedObject;
          this.selectedUsers = [];
          this.totalUsersCount = this.users.length > 0 ? this.users[0].totalRows : 0;
        }
        this.spinner.hide();
      }, () => {
        this.spinner.hide();
      });
    }
  }

  showAddQRCFormModel() {
    this.QRCForm.reset();
    this.QRCForm.patchValue({
      name: null,
      description: null,
      createdBy: this.user.id,
      updatedBy: null,
      categoryIds: null
    });
    this.showAddQRCForm = true;
  }

  showEditQRCFormModel() {
    this.QRCForm.reset();
    const qrc = this.qrcs.filter((x) => x.id === +this.qrc);
    this.QRCForm.patchValue({
      name: qrc[0].name,
      description: qrc[0].description,
      updatedBy: this.user.id,
      categoryIds: this.category
    });
    this.showAddQRCForm = true;
  }

  SubmitQRCForm(Form) {
    this.spinner.show();
    if (Form.valid) {
      this.QRCForm.patchValue({
        active: true
      });
      this.QRCService.postQrc(Form.value).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.QRCForm.reset();
          if(this.qrcFilterCategory !=0)
          this.getQrcList();
          this.messageService.add({severity: 'success', summary: 'QRC Added successfully', key: 'toast'});
          this.showAddQRCForm = false;
        } else {
          this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
        }
        this.spinner.hide();
      }, (error) => {
        this.messageService.add({severity: 'error', summary: 'Failed to add QRC', key: 'toast'});
        this.spinner.hide();
      });
    }
  }

  UpdateQRCForm(Form) {
    this.spinner.show();
    if (Form.valid) {
      this.QRCForm.patchValue({
        active: true
      });
      this.QRCService.patchQrc(Form.value, this.qrc).subscribe((res: any) => {
        if (res.hasSucceeded) {
          this.QRCForm.reset();
          this.getQrcList();
          this.messageService.add({severity: 'success', summary: 'QRC Updated successfully', key: 'toast'});
          this.showAddQRCForm = false;
        } else {
          this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
        }
        this.spinner.hide();
      }, (error) => {
        this.messageService.add({severity: 'error', summary: 'Failed to update QRC', key: 'toast'});
        this.spinner.hide();
      });
    }
  }

  showAddUsersFormModel() {
    this.newUsersPageNo = 1;
    this.newUsersPageSize = 10000;
    this.newUsers = [];
    this.selectedNewUsers = [];
    this.filterCategory = this.category;
    this.getNewUsers(this.qrc, this.category, this.newUsersPageNo, this.newUsersPageSize);
    this.showAddUsersForm = true;
  }

  filterCategoryChange() {
    this.getNewUsers(this.qrc, this.category, this.newUsersPageNo, this.newUsersPageSize);
  }

  getNewUsers(qrcId, categoryId, pageNo, pageSize) {
    if (qrcId && categoryId) {
      this.newUsers = [];
      this.newUsersTotal = 0;
      this.spinner.show();
      this.QRCService.fetchUsersToAdd(qrcId, categoryId, pageNo, pageSize, this.filterCategory).subscribe((res: any) => {
        this.spinner.hide();
        if (res.hasSucceeded) {
          this.patchEditUsers(res.returnedObject);
        }
      }, (error) => {
        this.spinner.hide();
      });
    }
  }

  patchEditUsers(users) {
    const newUsers = [];
    this.users.forEach((user) => {
      newUsers.push(user);
    });
    users.forEach((user) => {
      if (!newUsers.find(x => x.userId === user.userId)) {
        newUsers.push(user);
      }
    });
    if (users.length > 0) {
      this.newUsersTotal = users[0].totalRows;
    } else {
      this.newUsersTotal = 0;
    }
    this.selectedNewUsers = this.users;
    this.newUsersFirst = 0;
    this.newUsersTotal += this.users.length;
    this.newUsers = newUsers;
  }

  onPaginationChange(event) {
    if (event.first > 0) {
      this.getNewUsers(this.qrc, this.category, (event.first / event.rows) + 1, this.newUsersPageSize);
    } else {
      this.getNewUsers(this.qrc, this.category, 1, this.newUsersPageSize);
    }
  }

  addUsers(users) {
    const oldUserList = [];
    if ((users.length) > 2) {
      if ((users.length) % 2 === 1) {
        this.users.forEach((item) => {
          if (!users.find(x => x.userId === item.userId)) {
            this.spinner.show();
            this.userArray.push({
              id: item.userId,
              status: false,
              call: true,
              failed: false
            });
            this.removeUserFromQRC(item);
          } else {
            oldUserList.push(item);
          }
        });
        const newUserList = [];
        users.forEach((item) => {
          if (!this.users.find(x => x.userId === item.userId)) {
            newUserList.push(item);
          }
        });
        if (newUserList.length > 0) {
          this.startDeleteCheck(newUserList);
        } else {
          this.messageService.add({severity: 'success', summary: 'QRC Updated successfully', key: 'toast'});
          this.showAddUsersForm = false;
          this.newUsersPageNo = 1;
          this.newUsersPageSize = 10000;
          this.newUsers = [];
          this.selectedNewUsers = [];
          this.getQRCUserList();
        }
      } else {
        this.messageService.add({
          severity: 'error',
          summary: 'QRC require odd number of users, please add/remove a user',
          key: 'toast'
        });
      }
    } else {
      this.messageService.add({severity: 'error', summary: 'QRC should have more than 2 users', key: 'toast'});
    }
  }

  addUserSubmit(users) {
    this.spinner.show();
    const data = [];
    users.forEach((item) => {
      data.push({
        userId: item.userId,
        qrcId: +this.qrc,
        categoryId: +this.category,
        createdBy: this.profileService.user.id,
        emailUrl: environment.userClientUrl + 'dashboard/qrc'
      });
    });
    this.QRCService.postQRCUsers(data).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.messageService.add({severity: 'success', summary: 'QRC Updated successfully', key: 'toast'});
        this.showAddUsersForm = false;
        this.newUsersPageNo = 1;
        this.newUsersPageSize = 10000;
        this.newUsers = [];
        this.selectedNewUsers = [];
        this.getQRCUserList();
      } else {
        this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
      }
      this.spinner.hide();
    }, (error) => {
      this.messageService.add({severity: 'error', summary: 'Failed to update QRC', key: 'toast'});
      this.spinner.hide();
    });
  }

  cancelAddusers() {
    this.showAddUsersForm = false;
    this.newUsersPageNo = 1;
    this.newUsersPageSize = 10000;
    this.newUsers = [];
    this.selectedNewUsers = [];
  }

  startDeleteCheck(users) {
    if (this.userArray.filter(x => x.status === false).length === 0 && this.userArray.filter(x => x.call === true).length === 0 &&
    this.userArray.filter(x => x.failed === true).length === 0) {
      this.spinner.hide();
      this.userArray = [];
      this.selectedUsers = [];
      this.addUserSubmit(users);
    } else if (this.userArray.filter(x => x.call === true).length === 0 && this.userArray.filter(x => x.failed === true).length > 0) {
      this.messageService.add({
        severity: 'success',
        summary: this.userArray.filter(x => x.status === true).length + ' out of ' + this.userArray.length + ' Removed successfully',
        key: 'toast'
      });
      this.spinner.hide();
      this.userArray = [];
      this.selectedUsers = [];
      this.addUserSubmit(users);
    } else {
      setTimeout(() => {
        this.startDeleteCheck(users);
      }, 500);
    }
  }

  removeUserFromQRC(user) {
    this.QRCService.patchQRCUsers({
      userId: user.userId,
      qrcId: +this.qrc,
      categoryId: +this.category,
    }).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.userArray.forEach((item) => {
          if (item.id === user.userId) {
            item.status = true;
            item.call = false;
            item.failed = false;
          }
        });
      } else {
        this.userArray.forEach((item) => {
          if (item.id === user.userId) {
            item.call = false;
            item.failed = true;
          }
        });
      }
      this.spinner.hide();
    }, (error) => {
      this.userArray.forEach((item) => {
        if (item.id === user.userId) {
          item.call = false;
          item.failed = true;
        }
      });
    });
  }

  showUserProfile(id) {
    this.spinner.show();
    this.QRCService.getUserById(id).subscribe((res: any) => {
      if (res.hasSucceeded) {
        this.publicUser = res.returnedObject;
        this.getInterestSubject(this.publicUser);
        this.userProfile = true;
      } else {
        this.publicUser = null;
        this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
      }
      this.spinner.hide();
    }, (error) => {
      this.publicUser = null;
      this.messageService.add({severity: 'error', summary: 'Failed to load user data', key: 'toast'});
      this.spinner.hide();
    });
  }

  DeleteQRC() {
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this QRC?',
      accept: () => {
        this.spinner.show();
        this.QRCService.deleteQrc(this.qrc).subscribe((res: any) => {
          if (res.hasSucceeded) {
            this.messageService.add({severity: 'success', summary: 'Successfully deleted QRC', key: 'toast'});
            this.getQrcList();
          } else {
            this.messageService.add({severity: 'error', summary: res.message, key: 'toast'});
          }
          this.spinner.hide();
        }, (error) => {
          this.messageService.add({severity: 'error', summary: 'Failed to delete QRC', key: 'toast'});
          this.spinner.hide();
        });
      }
    });
  }

  getInterestSubject(data:any){    
    var splitData = data.subjectsInterested.split(',');    
    this.subjectsInterested ='';
    for(let i=0;i<splitData.length;i++){
      if(i==0){
        this.subjectsInterested += this.Allcategories.filter(x => x.id == splitData[i])[0].name;
      }
      else{
        this.subjectsInterested += ','+this.Allcategories.filter(x => x.id == splitData[i])[0].name;
      }
    }
  }

}
