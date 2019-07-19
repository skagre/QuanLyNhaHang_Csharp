CREATE DATABASE QuanLyNhaHang
GO
USE	QuanLyNhaHang
GO	
CREATE TABLE Accounts (
	id INT IDENTITY PRIMARY KEY,
	Username VARCHAR(255) NOT NULL UNIQUE,
	Password VARCHAR(255) NOT NULL,
	FullName NVARCHAR(255) NOT NULL,
	Sex NVARCHAR(10) NOT NULL,
	DateCreated	DATETIME DEFAULT CURRENT_TIMESTAMP,
	Salt VARCHAR(10) NOT NULL,
	Permission NVARCHAR(10) DEFAULT N'User',
	AccountStatus NVARCHAR(20) DEFAULT N'Hoạt động' 
)
INSERT INTO dbo.Accounts(Username, Password, FullName, Sex, Salt, Permission) VALUES
('admin', '99a978787b84e1a426c5f883866a0c9f', N'Admin', N'Nam', 221298 , N'Admin'),
('xuanbac', '68e54a5285943f1304d17dd6063f961b', N'Nguyễn Xuân Bắc', N'Nam', 729900, N'Admin'),
('thutrinh', '0a89aeb97caa8c043b49121223ee5b2e', N'Nguyễn Thị Thu Trinh', N'Nữ', 150799, N'Admin'),
('chauht', 'a87eb87023b8f37fd1f9c760d489215a', N'Lê Ngọc Châu', N'Nam', 220899, N'Admin'),
('thodz', '3adc08f023489079235b7e6e77636b56', N'Nguyễn Văn Thọ', N'Nam', 629900, N'Admin')
GO
INSERT INTO dbo.Accounts(Username, Password, FullName, Sex, Salt) VALUES
('dieulinh', '89de312bd03ea40d7b0ece59878f0365', N'Dương Thị Diệu Linh', N'Nữ', 236985),
('ngatran', 'd35ffb2686da392e2340fe287fa388dd', N'Trần Thị Nga', N'Nữ', 123456),
('mylinh', '5cc975121948f563fa7c93b79c68a6ca', N'Nguyễn Thị Mỹ Linh', N'Nữ', 456123),
('huyen', '654e982f89034661fb5455135abad960', N'Huyền', N'Nữ', 123444),
('thaibinh', 'c2308fdde7f92fde6234d5cc203d195e', N'Trần Thị Thái Bình', N'Nữ', 271299),
('ngocmai', '82179f6ad76d0dc89b3dad4b976da3f7', N'Nguyễn Ngọc Mai', N'Nữ', 545556),
('thanhnhan', '2dd3609101e10256fa7ec874f287e041', N'Nguyễn Thị Thanh Nhàn', N'Nữ', 112233),
('thuhien', '6051f9e048507a6b4da50c3a53792f59', N'Nguyễn Thị Thu Hiền', N'Nữ', 654777),
('ngocbich', '333f532d989f865d9411a909f5c03286', N'Vũ Ngọc Bích', N'Nữ', 147258),
('hoangyen', '477e223c40068089bea753b540e9c460', N'Mọt ngôn tình :)', N'Nữ', 159357),
('chuchu', 'a40938d056a8618047421ea7e3621b40', N'Hạ Linh <3', N'Nữ', 211299)
GO
CREATE TABLE Dining_Table (
	id INT IDENTITY PRIMARY KEY,
	DiningTableName NVARCHAR(10) NOT NULL UNIQUE,
	TableStatus NVARCHAR(10) DEFAULT N'Trống'
)
GO
--Insert DiningTable
DECLARE @i INT = 1
WHILE @i <= 30
BEGIN
	INSERT dbo.Dining_Table (DiningTableName)VALUES  ( N'Bàn ' + CAST(@i AS nvarchar(100)))
	SET @i = @i + 1
END
GO
CREATE TABLE Products (
	id INT IDENTITY PRIMARY KEY,
	ProductName NVARCHAR(255) NOT NULL UNIQUE,
	Category NVARCHAR(255) NOT NULL,
	Price FLOAT DEFAULT 0
)
GO
CREATE TABLE Bill (
	id INT IDENTITY PRIMARY KEY,
	PaymentDate DATETIME DEFAULT '1753-01-01',
	idDiningTable INT NOT NULL,
	BillStatus NVARCHAR(20) DEFAULT N'Chưa thanh toán',
	FOREIGN KEY (idDiningTable) REFERENCES dbo.Dining_Table(id)
)
GO
CREATE TABLE BillInfo (
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idProduct INT NOT NULL,
	Amount INT DEFAULT 0,
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idProduct) REFERENCES dbo.Products(id)
)
GO
CREATE PROC USP_InsertBillInfo
@idBill INT, @idProduct INT, @Amount INT
AS
BEGIN

	DECLARE @isExitsBillInfo INT
	DECLARE @ProductAmount INT = 1
	
	SELECT @isExitsBillInfo = id, @ProductAmount = dbo.BillInfo.Amount 
	FROM dbo.BillInfo 
	WHERE idBill = @idBill AND dbo.BillInfo.idProduct = @idProduct

	IF (@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newAmount INT = @idProduct + @Amount
		IF (@newAmount > 0)
			UPDATE dbo.BillInfo SET Amount = @ProductAmount + @Amount WHERE idProduct = @idProduct AND idBill = @idBill
		ELSE
			DELETE dbo.BillInfo WHERE idBill = @idBill AND idProduct = @idProduct
	END
	ELSE
	BEGIN
		INSERT	dbo.BillInfo
		        ( idBill, idProduct, Amount )
		VALUES  ( @idBill, -- idBill - int
		          @idProduct, -- idProduct - int
		          @Amount  -- Amount - int
		        )       
	END
END
GO
CREATE TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = idBill FROM Inserted
	
	DECLARE @idDiningTable INT
	
	SELECT @idDiningTable = idDiningTable FROM dbo.Bill WHERE id = @idBill AND BillStatus = N'Chưa thanh toán'
	
	UPDATE dbo.Dining_Table SET TableStatus = N'Có khách' WHERE id = @idDiningTable
END
GO
CREATE TRIGGER UTG_UpdateBill
ON dbo.Bill FOR UPDATE
AS
BEGIN
	DECLARE @idBill INT
	
	SELECT @idBill = id FROM Inserted	
	
	DECLARE @idDiningTable INT
	
	SELECT @idDiningTable = idDiningTable FROM dbo.Bill WHERE id = @idBill
	
	DECLARE @count int = 0
	
	SELECT @count = COUNT(*) FROM dbo.Bill WHERE idDiningTable = @idDiningTable AND BillStatus = N'Chưa thanh toán'
	
	IF (@count = 0)
	BEGIN
		UPDATE dbo.Dining_Table SET TableStatus = N'Trống' WHERE id = @idDiningTable
	END
END
GO
CREATE PROC USP_SwitchTabel
@id1 INT, @id2 int
AS BEGIN

	DECLARE @idBillofTable1 int
	DECLARE @idBillofTable2 INT
	
	DECLARE @checkEmptyTable1 INT = 1
	DECLARE @checkEmptyTable2 INT = 1
	
	SELECT @idBillofTable1 = id FROM dbo.Bill WHERE idDiningTable = @id1 AND BillStatus = N'Chưa thanh toán'
	SELECT @idBillofTable2 = id FROM dbo.Bill WHERE idDiningTable = @id2 AND BillStatus = N'Chưa thanh toán'
	
		
	IF (@idBillofTable1 IS NULL)
	BEGIN
		INSERT	dbo.Bill
		        ( idDiningTable ,
		          BillStatus
		        )
		VALUES  ( @id1 , -- idDiningTable - int
		          N'Chưa thanh toán'  -- BillStatus - nvarchar(20)
		        )
		        
		SELECT @idBillofTable1 = MAX(id) FROM dbo.Bill WHERE idDiningTable = @id1 AND BillStatus = N'Chưa thanh toán'
		
	END
	
	SELECT @checkEmptyTable1 = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBillofTable1
	
	IF (@idBillofTable2 IS NULL)
	BEGIN
		INSERT	dbo.Bill
		        ( idDiningTable ,
		          BillStatus
		        )
		VALUES  ( @id2 , -- idDiningTable - int
		          N'Chưa thanh toán'  -- BillStatus - nvarchar(20)
		        )
		SELECT @idBillofTable2 = MAX(id) FROM dbo.Bill WHERE idDiningTable = @id2 AND BillStatus = N'Chưa thanh toán'
		
	END
	
	SELECT @checkEmptyTable2 = COUNT(*) FROM dbo.BillInfo WHERE idBill = @idBillofTable2

	SELECT id INTO Temp FROM dbo.BillInfo WHERE idBill = @idBillofTable2
	
	UPDATE dbo.BillInfo SET idBill = @idBillofTable2 WHERE idBill = @idBillofTable1
	
	UPDATE dbo.BillInfo SET idBill = @idBillofTable1 WHERE id IN (SELECT * FROM Temp)
	
	DROP TABLE Temp
	
	IF (@checkEmptyTable1 = 0)
		UPDATE dbo.Dining_Table SET TableStatus = N'Trống' WHERE id = @id2
		
	IF (@checkEmptyTable2= 0)
		UPDATE dbo.Dining_Table SET TableStatus = N'Trống' WHERE id = @id1
END
GO
INSERT INTO dbo.Products(ProductName, Category, Price) VALUES 
( N'Soup bắp cua', N'Các món soup', 220000),
( N'Soup măng tây cua', N'Các món soup', 220000),
( N'Soup tôm cua', N'Các món soup', 220000),
( N'Soup bóng cá, cua', N'Các món soup', 220000),
( N'Soup bóng cá, nấm tuyết', N'Các món soup', 250000),
( N'Soup hải sản', N'Các món soup', 250000),
( N'Soup hải sản, óc heo', N'Các món soup', 250000),
( N'Soup sò điệp, hải sản', N'Các món soup', 250000),
( N'Soup tơ vương', N'Các món soup', 250000),
( N'Soup tôm quý hải vị', N'Các món soup', 250000),
( N'Soup tứ vị', N'Các món soup', 250000),
( N'Soup trùng dương', N'Các món soup', 250000),
( N'Soup tam tơ', N'Các món soup', 250000),
( N'Soup tôm phù dung', N'Các món soup', 300000),
( N'Soup cua, bào ngư', N'Các món soup', 300000),
( N'Soup gà, vi cá', N'Các món soup', 550000),
( N'Soup bào ngư, vi cá, hải sâm', N'Các món soup', 600000),

( N'Gỏi miến trái tim', N'Các món gỏi', 250000),
( N'Gỏi chân gà rút xương', N'Các món gỏi', 250000),
( N'Gỏi bao tử, tôm', N'Các món gỏi', 250000),
( N'Gỏi bồn bồn tôm, thịt', N'Các món gỏi', 250000),
( N'Gỏi củ hũ dừa tôm, thịt', N'Các món gỏi', 250000),
( N'Gỏi ngó sen tôm, thịt', N'Các món gỏi', 250000),
( N'Gỏi bò tái chanh', N'Các món gỏi', 300000),
( N'Gỏi ngó sen tôm, mực', N'Các món gỏi', 300000),
( N'Gỏi sụn rong biển tôm, mực', N'Các món gỏi', 300000),
( N'Gỏi tiến vua tôm, thịt', N'Các món gỏi', 300000),
( N'Gỏi uyên ương', N'Các món gỏi', 250000),
( N'Gỏi cung đình hải sản', N'Các món gỏi', 300000),
( N'Gỏi mực Thái', N'Các món gỏi', 300000),
( N'Gỏi tôm, mực Thái', N'Các món gỏi', 300000),
( N'Gỏi tôm bóp thấu', N'Các món gỏi', 300000),
( N'Gỏi bò bóp thấu', N'Các món gỏi', 300000),
( N'Gỏi sứa', N'Các món gỏi', 300000),
( N'Gỏi bò trộn cái mầm', N'Các món gỏi', 300000),
( N'Gỏi gà Long Phụng', N'Các món gỏi', 350000),

( N'Sườn heo lagu + bánh mì', N'Các món heo', 330000),
( N'Sườn heo nấu bắp non + bánh mì', N'Các món heo', 330000),
( N'Sường heo nấu đậu + bánh mì', N'Các món heo', 330000),
( N'Sườn heo tứ sắc', N'Các món heo', 330000),
( N'Sườn heo nướng Nga', N'Các món heo', 330000),
( N'Sườn heo non nướng Hồng Kông', N'Các món heo', 330000),
( N'Sườn heo non nướng Mã Lai', N'Các món heo', 330000),
( N'Lưỡi heo hầm tứ sắc + bánh mì', N'Các món heo', 350000),
( N'Lưỡi heo nấu đậu + bánh mì', N'Các món heo', 350000),
( N'Lưỡi heo rooti + bánh mì', N'Các món heo', 350000),
( N'Giò heo muối Minh Long', N'Các món heo', 350000),
( N'Giò heo kim chi', N'Các món heo', 350000),
( N'Giò heo hầm đông cô', N'Các món heo', 350000),
( N'Giò heo hầm tóc tiên', N'Các món heo', 350000),
( N'Heo nướng Nga', N'Các món heo', 350000),
( N'Dồi trường hấp gừng', N'Các món heo', 330000),
( N'Bao tử hấp gừng', N'Các món heo', 330000),
( N'Bao tử hầm tiêu', N'Các món heo', 350000),
( N'Heo rừng xào lăn', N'Các món heo', 350000),
( N'Heo rừng hấp gừng', N'Các món heo', 350000),
( N'Heo sữa quay (1/2 con) + bánh bao', N'Các món heo', 550000),
( N'Heo tộc quay (1/2 con) + bánh bao', N'Các món heo', 750000),

( N'Gà hấp bẹ xanh', N'Các món gà', 350000),
( N'Gà hấp hành', N'Các món gà', 350000),
( N'Gà sốt cải Hồng Kông', N'Các món gà', 360000),
( N'Gà tiềm hạt sen + mì', N'Các món gà', 360000),
( N'Gà tiềm nấm hương + mì', N'Các món gà', 360000),
( N'Gà tiềm ngũ quả + mì', N'Các món gà', 360000),
( N'Gà tiềm thuốc bắc + mì', N'Các món gà', 360000),
( N'Gà tiềm táo đỏ + mì', N'Các món gà', 360000),
( N'Gà uyên ương', N'Các món gà', 360000),
( N'Cà ri gà + bánh mì', N'Các món gà', 360000),
( N'Gà hầm đậu trắng', N'Các món gà', 360000),
( N'Gà hầm patê + bánh mì', N'Các món gà', 360000),
( N'Gà hầm sâm + bánh mì', N'Các món gà', 360000),
( N'Gà hầm nấm tươi + bánh mì', N'Các món gà', 360000),
( N'Gà đốt muối + xôi trắng', N'Các món gà', 360000),
( N'Gà hấp mắm nhỉ + xôi trắng', N'Các món gà', 360000),
( N'Gà hấp ớt hiểm + xôi trắng', N'Các món gà', 360000),
( N'Gà hấp lá chanh + xôi trắng', N'Các món gà', 360000),
( N'Gà hấp lá chanh + xôi gấc 3 tầng', N'Các món gà', 360000),
( N'Gà nướng lu + xôi phồng', N'Các món gà', 360000),
( N'Gà quay + bánh bao', N'Các món gà', 360000),
( N'Gà quay hành thơm', N'Các món gà', 360000),
( N'Gà xối mỡ + xôi gấc', N'Các món gà', 360000),
( N'Gà bó xôi chiên', N'Các món gà', 380000),

( N'Bò hầm tứ sắc + bánh mì', N'Các món bò', 350000),
( N'Bò hon nước dừa + bánh mì', N'Các món bò', 350000),
( N'Bò lagu + bánh mì', N'Các món bò', 350000),
( N'Bò nấu bia + bánh mì', N'Các món bò', 350000),
( N'Bò nấu đậu + bánh mì', N'Các món bò', 350000),
( N'Bò nấu đốp + bánh mì', N'Các món bò', 350000),
( N'Bò nấu tiêu xanh + bánh mì', N'Các món bò', 350000),
( N'Bò sốt patê + bánh mì', N'Các món bò', 350000),
( N'Bò cuốn pho mát + bánh mì', N'Các món bò', 350000),
( N'Bò né', N'Các món bò', 330000),

( N'Thỏ quay + bánh bao', N'Các món thỏ', 350000),
( N'Thỏ xào lăn + bánh mì', N'Các món thỏ', 350000),
( N'Thỏ lagu + bánh mì', N'Các món thỏ', 350000),
( N'Thỏ cà ri + bánh mì', N'Các món thỏ', 350000),
( N'Thỏ sốt rượu vang + bánh mì', N'Các món thỏ', 350000),
( N'Thỏ nướng muối Thái', N'Các món thỏ', 350000),
( N'Thỏ nướng Hồng Kông', N'Các món thỏ', 350000),

( N'Vịt hấp ớt hiểm + xôi trắng', N'Các món vịt', 330000),
( N'Vịt nướng chao + bánh bao', N'Các món vịt', 330000),
( N'Vịt nướng ngũ vị + bánh bao', N'Các món vịt', 330000),
( N'Vịt nướng là móc mật + xôi trắng', N'Các món vịt', 350000),
( N'Vịt quay Bắc Kinh + bánh bao', N'Các món vịt', 330000),
( N'Vịt quay Tứ Xuyên + bánh bao', N'Các món vịt', 330000),
( N'Vịt nấu Civê + bánh mì', N'Các món vịt', 330000),
( N'Vịt quay chao', N'Các món vịt', 350000),
( N'Vịt nấu khoai sọ', N'Các món vịt', 350000),
( N'Vịt tiềm hạt sen + mì', N'Các món vịt', 350000),
( N'Vịt nấu chao + bún', N'Các món vịt', 350000),
( N'Vịt tiềm đẳng sâm + mì', N'Các món vịt', 350000),
( N'Vịt tiềm ngũ quả + mì', N'Các món vịt', 400000),
( N'Vịt tiềm thuốc bắc + mì', N'Các món vịt', 400000),
( N'Đùi vịt tiềm nấm hương', N'Các món vịt', 500000),

( N'Bồ câu quay + bánh bao', N'Các món bồ câu', 500000),
( N'Bồ câu nướng ngũ vị + bánh bao', N'Các món bồ câu', 500000),
( N'Bồ câu nướng lá chanh + bánh bao', N'Các món bồ câu', 500000),
( N'Bồ câu rô ti nước dừa', N'Các món bồ câu', 500000),
( N'Bồ câu tiềm thuốc bắc', N'Các món bồ câu', 500000),
( N'Bồ câu tiềm hạt sen', N'Các món bồ câu', 500000),
( N'Bồ câu hầm tiêu xanh', N'Các món bồ câu', 500000),
( N'Bồ câu hầm sâm', N'Các món bồ câu', 500000),

( N'Cá diêu hồng nướng giấy bạc', N'Các món cá', 300000),
( N'Cá diêu hồng chưng tương', N'Các món cá', 300000),
( N'Cá diêu hồng hấp Hồng Kông', N'Các món cá', 300000),
( N'Cá diêu hồng hấp đông cô', N'Các món cá', 300000),
( N'Cá diêu hồng hấp kỳ lân', N'Các món cá', 300000),
( N'Cá diêu hồng sôt ngũ liễu', N'Các món cá', 250000),
( N'Cá diêu hồng sốt cam', N'Các món cá', 260000),
( N'Cá diêu hồng sốt chua ngọt', N'Các món cá', 260000),
( N'Cá lóc hấp Hồng Kông', N'Các món cá', 260000),
( N'Cá lóc hấp nấm tươi', N'Các món cá', 260000),
( N'Cá lóc nhồi thịt + bánh hỏi', N'Các món cá', 300000),
( N'Cá saba nướng bơ tỏi', N'Các món cá', 300000),
( N'Cá saba nướng giấy bạc', N'Các món cá', 300000),
( N'Cá tai tượng sốt ngũ liễu', N'Các món cá', 300000),
( N'Cá tai tượng chiên xù + bánh tráng', N'Các món cá', 300000),
( N'Cá tai tượng sốt chua ngọt', N'Các món cá', 300000),
( N'Cá tai tượng sốt camt', N'Các món cá', 300000),
( N'Cá tai tượng sốt dâu', N'Các món cá', 300000),
( N'Cá tai tượng sốt me', N'Các món cá', 300000),
( N'Cá chẽm sốt chua ngọt', N'Các món cá', 350000),
( N'Cá chẽm sốt dâu', N'Các món cá', 350000),
( N'Cá chẽm sốt chanh dây', N'Các món cá', 350000),
( N'Cá chẽm sốt cam', N'Các món cá', 350000),
( N'Cá chẽm sốt ngũ liễu', N'Các món cá', 350000),
( N'Cá chẽm hấp Hồng Kông', N'Các món cá', 350000),
( N'Cá chẽm chưng tương', N'Các món cá', 350000),
( N'Cá chẽm hấp kỳ lân', N'Các món cá', 350000),
( N'Cá tầm hấp Hồng Kông', N'Các món cá', 490000),
( N'Cá tầm ủ muối', N'Các món cá', 490000),
( N'Cá tầm chưng tương', N'Các món cá', 490000),
( N'Cá tầm hấp Singapo', N'Các món cá', 490000),
( N'Cá bống mú hấp Hồng Kông', N'Các món cá', 530000),
( N'Cá bống mú hấp kỳ lân', N'Các món cá', 530000),
( N'Cá bống mú chưng tương', N'Các món cá', 530000),
( N'Cá bống mú sốt cam', N'Các món cá', 530000),
( N'Cá bống mú sốt ngũ liễu', N'Các món cá', 530000),
( N'Cá bống mú ra khơi', N'Các món cá', 530000),
( N'Cá chình nướng muối ớt', N'Các món cá', 850000),
( N'Cá chình hấp sả', N'Các món cá', 900000),

( N'Tôm sú hấp bia', N'Các món tôm', 390000),
( N'Tôm sú hấp nước dừa', N'Các món tôm', 390000),
( N'Tôm sú ủ muối', N'Các món tôm', 390000),
( N'Tôm sú nướng muối ớt', N'Các món tôm', 400000),
( N'Tôm sú rang me', N'Các món tôm', 400000),
( N'Tôm sú rang muối Mã Lai', N'Các món tôm', 400000),
( N'Tôm cẩm tú', N'Các món tôm', 400000),
( N'Tôm sốt Tứ Xuyên', N'Các món tôm', 400000),
( N'Tôm hạnh nhân', N'Các món tôm', 400000),
( N'Tôm lăn cốm', N'Các món tôm', 400000),
( N'Tôm sốt chanh dây', N'Các món tôm', 400000),
( N'Tôm bách hoa', N'Các món tôm', 400000),
( N'Tôm chiên mè, dừa', N'Các món tôm', 400000),
( N'Tôm chiên bát trân', N'Các món tôm', 400000),
( N'Tôm lăn bột chiên giòn', N'Các món tôm', 400000),
( N'Tôm lăn bột chiên xù', N'Các món tôm', 400000),
( N'Tôm Phụng Hoàng', N'Các món tôm', 400000),
( N'Tôm sú sôt mayonnaise', N'Các món tôm', 400000),
( N'Tôm sú hoàng kim', N'Các món tôm', 400000),
( N'Cà ri tôm + bánh mì', N'Các món tôm', 400000),
( N'Tôm Đại Dương', N'Các món tôm', 430000),
( N'Tôm càng hấp bia', N'Các món tôm', 810000),
( N'Tôm càng sốt mayonnaise', N'Các món tôm', 810000),
( N'Tôm càng hấp nước dừa', N'Các món tôm', 810000),
( N'Tôm càng nướng', N'Các món tôm', 810000),
( N'Tôm càng rang me', N'Các món tôm', 810000),
( N'Tôm càng rang muối Mã Lai', N'Các món tôm', 810000),
( N'Tôm càng cháy tỏi kiểu Thái', N'Các món tôm', 810000),
( N'Tôm càng chiên nước mắm Thái', N'Các món tôm', 810000),
( N'Tôm càng sốt Singapore + bánh bao', N'Các món tôm', 810000),

( N'Cua lột chiên cốm', N'Các món cua', 290000),
( N'Cua lột tẩm bột, sốt bơ', N'Các món cua', 290000),
( N'Cua lột tứ sắc', N'Các món cua', 290000),
( N'Cua biển rang me', N'Các món cua', 290000),

( N'Bào ngư sốt ngồng cải Hồng Kông', N'Các món hải sản', 360000),
( N'Bào ngư sốt đông cô', N'Các món hải sản', 360000),
( N'Mực nướng muối Thái', N'Các món hải sản', 350000),
( N'Mực ống hấp Hồng Kông', N'Các món hải sản', 350000),
( N'Mực sốt đại dương', N'Các món hải sản', 350000),
( N'Mực uyên ương', N'Các món hải sản', 350000),
( N'Mực hấp Thái Lan', N'Các món hải sản', 350000),
( N'Hải sản xào nấm đông cô', N'Các món hải sản', 350000),
( N'Hải sản xào tổ chim', N'Các món hải sản', 350000),

( N'Cải rổ sốt đông cô', N'Các món rau', 200000),
( N'Cải ngồng sốt đông cô', N'Các món rau', 200000),
( N'Cải thìa sốt đông cô', N'Các món rau', 200000),
( N'Cải rổ sốt dầu hảo', N'Các món rau', 200000),
( N'Cải ngồng sốt dầu hảo', N'Các món rau', 200000),
( N'Cải thìa sốt dầu hảo', N'Các món rau', 200000),
( N'Cải rổ sốt tôm, mực', N'Các món rau', 300000),
( N'Cải ngồng sốt tôm, mực', N'Các món rau', 300000),
( N'Cải thìa sốt tôm, mực', N'Các món rau', 300000),
( N'Nấm đông cô tươi', N'Các món rau', 300000),
( N'Nấm kim châm', N'Các món rau', 300000),
( N'Nấm đùi gà sốt tim', N'Các món rau', 300000),
( N'Bông cải xào thập cẩm', N'Các món rau', 300000),
( N'Bông cải xào tôm, mực', N'Các món rau', 300000),
( N'Tổ chim hải sản', N'Các món rau', 350000),

( N'Lẩu cá diêu hồng nấu riêu + bún', N'Các món lẩu', 310000),
( N'Lẩu Miso + mì', N'Các món lẩu', 340000),
( N'Lẩu Triển Châu + bún', N'Các món lẩu', 340000),
( N'Lẩu thập cẩm + mì', N'Các món lẩu', 340000),
( N'Lẩu cá thác lác + bún', N'Các món lẩu', 340000),
( N'Lẩu cua đồng sườn non + bún', N'Các món lẩu', 340000),
( N'Lẩu hải vị Thái Lan + bún', N'Các món lẩu', 340000),
( N'Lẩu sống + mì', N'Các món lẩu', 340000),
( N'Lẩu Thái + bún', N'Các món lẩu', 340000),
( N'Lẩu hải sản + mì', N'Các món lẩu', 340000),
( N'Lẩu hải sản chua cay + bún', N'Các món lẩu', 340000),
( N'Lẩu hải sản hoa anh đào + bánh phở', N'Các món lẩu', 340000),
( N'Lẩu mực Thái + bún', N'Các món lẩu', 340000),
( N'Lẩu đồng nội + mì', N'Các món lẩu', 340000),
( N'Lẩu uyên ương + bún', N'Các món lẩu', 350000),
( N'Lẩu cua đồng bắp bò + bún', N'Các món lẩu', 350000),
( N'Lẩu hải sản kim chi + bún', N'Các món lẩu', 350000),
( N'Lẩu nấm hải sản + bún', N'Các món lẩu', 380000),
( N'Lẩu gà hoa anh đào + bánh phở', N'Các món lẩu', 360000),
( N'Lẩu gà nấu lá giang + bún', N'Các món lẩu', 360000),
( N'Lẩu gà cua đồng + bún', N'Các món lẩu', 360000),
( N'Lẩu gà Hơ Mông nấm nấm + bún', N'Các món lẩu', 400000),
( N'Lẩu tôm Thái + bún', N'Các món lẩu', 400000),
( N'Lẩu tôm càng Thái + bún', N'Các món lẩu', 850000),
( N'Lẩu cá lăng nấu măng chua + bún', N'Các món lẩu', 350000),
( N'Lẩu cá lăng nấu ngót + bún', N'Các món lẩu', 350000),
( N'Lẩu cá mú nấu măng chua + bún', N'Các món lẩu', 550000),
( N'Lẩu cá mú nấu tiêu + bún', N'Các món lẩu', 550000),
( N'Lẩu cá chình nấu riêu + bún', N'Các món lẩu', 900000),
( N'Lẩu cá chình nấu ngót + bún', N'Các món lẩu', 900000)

GO
CREATE PROC USP_DeleteProduct @id INT
AS BEGIN
	ALTER TABLE dbo.BillInfo NOCHECK CONSTRAINT ALL
	DELETE FROM dbo.Products WHERE id = @id
	ALTER TABLE dbo.BillInfo CHECK CONSTRAINT ALL
END
GO	
CREATE PROC USP_DeleteDiningTable @id INT
AS BEGIN
	ALTER TABLE dbo.Bill NOCHECK CONSTRAINT ALL
	DELETE FROM dbo.Dining_Table WHERE id = @id
	ALTER TABLE dbo.Bill CHECK CONSTRAINT ALL
END
